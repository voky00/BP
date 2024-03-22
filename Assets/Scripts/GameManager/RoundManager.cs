using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

public class RoundManager : MonoBehaviour
{
    bool taskRunning = false;
    public static int playerOnTurn = 0;
    public static bool diceIsSelected = false;
    public GameObject PlayerInfo;
    public GameObject JobSpawn;
    public GameObject BusinessSpawn;
    public GameObject gameMenu;
    public GameObject Tutorials;
    public GameObject nextPlayerWindow;
    public static int round = 0;
    public static int diceToPlay = 3;
    public static int figureOnTurn = 0;
    public static Dice selectedDice;
    public static bool[] tutorialsShown = { false, false, false, false, false };
    public enum phaseType { start, chooseDirection, moving, end };
    public static phaseType phase = phaseType.start;
    public static Player[] players = new Player[8];
    public TMP_Text PlayerNameText;
    public TMP_Text MoneyText;
    public TMP_Text RoundCounter;
    public TMP_Text FigureOnTurnText;
    public TMP_Text EducationText;

    public Button DiceRoll;
    public Button GoToWork;
    public Button BackToStudy;

    public Figure FgPrefab;
    public ColorManager colorManager;
    public Movement mv;
    public EndScreen endSc;

    public bool menuShown = false;
    public bool botMoving = false;

    public static int nextDelay = 2000;

    void Update()
    {
        if (!tutorialsShown[0])
        {
            Tutorials.transform.GetChild(0).gameObject.SetActive(true);
            tutorialsShown[0] = true;
        }

        if (Lobby.roundCount == round)
        {
            endSc.gameObject.SetActive(true);
            endSc.writeEndInfo();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuShown)
            {
                gameMenu.SetActive(false);
                menuShown = false;    
            }
            else
            {
                gameMenu.SetActive(true);
                menuShown = true;
            }
        }
        if (botMoving) return;
        if (players[playerOnTurn].isAi)
        {
            GoToWork.gameObject.SetActive(false);
            BackToStudy.gameObject.SetActive(false);
            DiceRoll.gameObject.SetActive(false);
            BotTurnAsync();
            return;
        }
        playerTurn();
    }

     void playerTurn()
     {
        Figure fg = players[playerOnTurn].Fg[figureOnTurn];

        if (fg != null)
        {
            MoneyText.SetText("peníze: " + fg.money + " Kè");
            switch (fg.education)
            {
                case 0: EducationText.SetText("vzdìlání - žádné"); break;
                case 1: EducationText.SetText("vzdìlání - základní škola"); 
                    if (!tutorialsShown[2])
                    {
                        Tutorials.transform.GetChild(2).gameObject.SetActive(true);
                        tutorialsShown[2] = true;
                    }
                    break;
                case 2: EducationText.SetText("vzdìlání - støední škola"); break;
                case 3: EducationText.SetText("vzdìlání - vysoká škola"); break;
            }
        }
        if (round == 0 || taskRunning) return;
        switch (phase)
        {
            case phaseType.start:
                startPhase(fg);
                break;

            case phaseType.chooseDirection:
                choosingPhase(fg);
                break;

            case phaseType.moving:
                movingPhase(fg);
                break;

            case phaseType.end:
                fg.GetComponent<Animator>().SetBool("OnTurn", false);
                NextFigure();
                phase = phaseType.start;
                break;

            default:
                phase = phaseType.start;
                break;
        }
     }
    void startPhase(Figure fg)
    {
        if (!tutorialsShown[1])
        {
            Tutorials.transform.GetChild(1).gameObject.SetActive(true);
            tutorialsShown[1] = true;
        }
        Movement.lastDirection = Movement.direction.none;
        if (fg.education > 0 && fg.studying)
        {
            GoToWork.gameObject.SetActive(true);
            BackToStudy.gameObject.SetActive(false);
        }
        else if (!fg.studying)
        {
            BackToStudy.gameObject.SetActive(true);
            GoToWork.gameObject.SetActive(false);
        }
        DiceRoll.gameObject.SetActive(true);
        fg.GetComponent<Animator>().SetBool("OnTurn", true);

        if (fg.studying)
        {
            DiceThrower.amoutOfDice = 3;
            diceToPlay = 3;
        }
        else
        {
            DiceThrower.amoutOfDice = fg.education;
            diceToPlay = fg.education;
        }
    }
    void choosingPhase(Figure fg)
    {
        
        if (diceIsSelected)
        {
            if (fg.studying)
            {
                if (fg.positionX < 20)
                    fg.transform.GetChild(4).gameObject.SetActive(true);
                else if (fg.positionX < 40)
                    fg.transform.GetChild(1).gameObject.SetActive(true);
                else if (fg.positionX < 60)
                    fg.transform.GetChild(3).gameObject.SetActive(true);
                else if (fg.positionX < 80)
                    fg.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (fg.status == Figure.statusType.job)
            {
                if (fg.positionX < 8 || fg.positionX > 55)
                    fg.transform.GetChild(4).gameObject.SetActive(true);
                else if (fg.positionX < 24)
                    fg.transform.GetChild(1).gameObject.SetActive(true);
                else if (fg.positionX < 40)
                    fg.transform.GetChild(3).gameObject.SetActive(true);
                else if (fg.positionX < 56)
                    fg.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (fg.status == Figure.statusType.business)
            {
                if (fg.positionX + selectedDice.value < 13 && Movement.lastDirection != Movement.direction.left)
                    fg.transform.GetChild(3).gameObject.SetActive(true); //right
                if (fg.positionX - selectedDice.value >= 0 && Movement.lastDirection != Movement.direction.right)
                    fg.transform.GetChild(4).gameObject.SetActive(true); //left
                if (fg.positionY + selectedDice.value < 13 && Movement.lastDirection != Movement.direction.down)
                    fg.transform.GetChild(1).gameObject.SetActive(true); //up
                if (fg.positionY - selectedDice.value >= 0 && Movement.lastDirection != Movement.direction.up)
                    fg.transform.GetChild(2).gameObject.SetActive(true); //down
            }
        }
    }
    void movingPhase(Figure fg)
    {
        if (Movement.arrowClidked)
        {
            if (fg.studying)
            {
                mv.studyMove(selectedDice.value);
            }
            else if (fg.status == Figure.statusType.job)
            {
                int value = 0;
                for (int i = 0; i < DiceThrower.spawnedDices.Length; i++)
                    if (DiceThrower.spawnedDices[i] != null)
                    {
                        value += DiceThrower.spawnedDices[i].GetComponent<Dice>().value;
                        Destroy(DiceThrower.spawnedDices[i]);
                    }
                mv.jobMove(value);
            }
            else if (fg.status == Figure.statusType.business)
            {
                mv.businessMove(selectedDice.value);
            }
        }
    }
    async Task BotTurnAsync()
    {
        EducationText.SetText("");
        MoneyText.SetText("");

        if (round == 0)
        {
            players[playerOnTurn].Fg[figureOnTurn] = Instantiate(FgPrefab, transform.position, Quaternion.identity);
            colorManager.SetColor(players[playerOnTurn].Fg[figureOnTurn].GetComponentInChildren<Renderer>(), players[playerOnTurn].color);
            players[playerOnTurn].Fg[figureOnTurn].education = Random.Range(1, 3);
            MoveToBusiness();
            players[playerOnTurn].Fg[figureOnTurn].positionX = 6; players[playerOnTurn].Fg[figureOnTurn].positionY = 0;

            players[playerOnTurn].Fg[figureOnTurn + 1] = Instantiate(FgPrefab, transform.position, Quaternion.identity);
            colorManager.SetColor(players[playerOnTurn].Fg[figureOnTurn + 1].GetComponentInChildren<Renderer>(), players[playerOnTurn].color);
            players[playerOnTurn].Fg[figureOnTurn + 1].gameObject.SetActive(false);

            
            figureOnTurn++;
            NextFigure();
            
        }
        else
        {
            Figure fg = players[playerOnTurn].Fg[figureOnTurn];

            botMoving = true;
            diceToPlay = fg.education;
            int randomDiceValue;
            int direction;

            while (diceToPlay != 0)
            {
                randomDiceValue = Random.Range(1, 6);
                //Debug.Log(diceToPlay + ", " + randomDiceValue);
                bool notMoved = true;

                while (notMoved)
                {
                    direction = Random.Range(1, 4);
                    //Debug.Log(direction);

                    switch (direction)
                    {
                        case 1:
                            if (fg.positionX + randomDiceValue < 13 && Movement.lastDirection != Movement.direction.left)
                            {
                                notMoved = false;
                                Movement.moveDirection = Movement.direction.right;
                                await mv.businessMove(randomDiceValue);
                            }

                            break;
                        case 2:
                            if (fg.positionX - randomDiceValue >= 0 && Movement.lastDirection != Movement.direction.right)
                            {
                                notMoved = false;
                                Movement.moveDirection = Movement.direction.left;
                                await mv.businessMove(randomDiceValue);
                            }
                            break;
                        case 3:
                            if (fg.positionY + randomDiceValue < 13 && Movement.lastDirection != Movement.direction.down)
                            {
                                notMoved = false;
                                Movement.moveDirection = Movement.direction.up;
                                await mv.businessMove(randomDiceValue);
                            }
                            break;
                        case 4:
                            if (fg.positionY - randomDiceValue >= 0 && Movement.lastDirection != Movement.direction.up)
                            {
                                notMoved = false;
                                Movement.moveDirection = Movement.direction.down;
                                await mv.businessMove(randomDiceValue);
                            }
                            break;
                    }
                }
            }

            botMoving = false;
            figureOnTurn++;
            NextFigure();
            
        }
        
    }
    public void MoveToBusiness()
    {
        if (!tutorialsShown[4] && !players[playerOnTurn].isAi)
        {
            Tutorials.transform.GetChild(4).gameObject.SetActive(true);
            tutorialsShown[4] = true;
        }
        players[playerOnTurn].Fg[figureOnTurn].transform.position = BusinessSpawn.transform.position;
        players[playerOnTurn].Fg[figureOnTurn].studying = false;
        players[playerOnTurn].Fg[figureOnTurn].status = Figure.statusType.business;
        for (int i = 1; i < 5; i++)
            players[playerOnTurn].Fg[figureOnTurn].transform.GetChild(i).gameObject.SetActive(false);
    }
    public void MoveToJob(GameObject where)
    {
        if (!tutorialsShown[3])
        {
            Tutorials.transform.GetChild(3).gameObject.SetActive(true);
            tutorialsShown[3] = true;
        }
        players[playerOnTurn].Fg[figureOnTurn].transform.position = where.transform.position;
        players[playerOnTurn].Fg[figureOnTurn].studying = false;
        players[playerOnTurn].Fg[figureOnTurn].status = Figure.statusType.job;
        for (int i = 1; i < 5; i++)
            players[playerOnTurn].Fg[figureOnTurn].transform.GetChild(i).gameObject.SetActive(false);
    }
    public void SetActiveWork()
    {
        Figure fg1 = players[playerOnTurn].Fg[0];
        Figure fg2 = players[playerOnTurn].Fg[1];
        Figure fg = players[playerOnTurn].Fg[figureOnTurn];

        if (fg1.status == Figure.statusType.none && fg2.status == Figure.statusType.none)
        {
            JobSpawn.SetActive(true);
            BusinessSpawn.SetActive(true);
        }
        else if (fg.status == Figure.statusType.job)
            JobSpawn.SetActive(true);
        else if (fg.status == Figure.statusType.business)
            BusinessSpawn.SetActive(true);
        else if (fg1.status == Figure.statusType.job || fg2.status == Figure.statusType.job)
            BusinessSpawn.SetActive(true);
        else
            JobSpawn.SetActive(true);
    }
    public void SetActiveStudy()
    {
        for (int i = 0; i < StudySpawns.studySpawns.Length; i++)
            StudySpawns.studySpawns[i].SetActive(true);
    }
    private void Awake()
    {
        
        /*////////////////////////////       test
        Lobby.roundCount = 10;
        for (int i = 0; i < 2; i++)
        {
            players[i] = new Player();
            players[i].playerName = "Player" + i;
            players[i].color = i;
        }
        players[0].isAi = true;
        /*///////////////////////////


        RoundCounter.SetText("Poèet kol: " + round + "/" + Lobby.roundCount);
    }
    public void spawnFigure(FigureSpawn where)
    {
        if (round != 0) 
        {
            DiceRoll.gameObject.SetActive(false);
            players[playerOnTurn].Fg[figureOnTurn].transform.position = where.transform.position;
            players[playerOnTurn].Fg[figureOnTurn].studying = true;
            setPositionX(where.x);
            for (int i = 0; i < StudySpawns.studySpawns.Length; i++)
            {
                StudySpawns.studySpawns[i].GetComponent<Renderer>().enabled = false;
                StudySpawns.studySpawns[i].SetActive(false);
            }
            for (int i = 1; i < 5; i++)
                players[playerOnTurn].Fg[figureOnTurn].transform.GetChild(i).gameObject.SetActive(false);

            phase = phaseType.end;

            return; 
        }

        players[playerOnTurn].Fg[figureOnTurn] = Instantiate(FgPrefab, where.transform.position, Quaternion.identity);
        colorManager.SetColor(players[playerOnTurn].Fg[figureOnTurn].GetComponentInChildren<Renderer>(), players[playerOnTurn].color);
        setPositionX(where.x);
        NextFigure();
        
    }

    public void setPositionX(int x)
    {
        players[playerOnTurn].Fg[figureOnTurn].positionX = x;
    }
    public void setPositionY(int y)
    {
        players[playerOnTurn].Fg[figureOnTurn].positionY = y;
    }

    public async void NextFigure()
    {
        if (!players[playerOnTurn].isAi && round != 0)
        {
            taskRunning = true;
            BackToStudy.gameObject.SetActive(false);
            GoToWork.gameObject.SetActive(false);
            await Task.Delay(nextDelay);
            taskRunning = false;
        }
           
        figureOnTurn++;
        if (figureOnTurn == 2)
        {
            figureOnTurn = 0;
            playerOnTurn++;
            if (playerOnTurn > Lobby.playerCount - 1)
            {
                playerOnTurn = 0;
                if (round == 0)
                for (int i = 0; i < StudySpawns.studySpawns.Length; i++)
                {
                    StudySpawns.studySpawns[i].GetComponent<Renderer>().enabled = false;
                    StudySpawns.studySpawns[i].SetActive(false);
                }
                round++;
            }
            if (!players[playerOnTurn].isAi)
            {
                nextPlayerWindow.SetActive(true);
                nextPlayerWindow.GetComponentInChildren<TMP_Text>().SetText(players[playerOnTurn].playerName + " je na øadì.");
            }
        }
        RoundCounter.SetText("Poèet kol: " + round + "/" + Lobby.roundCount);  
        PlayerNameText.SetText(players[playerOnTurn].playerName);    
        FigureOnTurnText.SetText("figurka " + (figureOnTurn + 1));
        if (players[playerOnTurn].isAi)
        {
            PlayerNameText.SetText(players[playerOnTurn].playerName + " (bot)");
            FigureOnTurnText.SetText("");
        }
    }
}
