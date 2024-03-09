using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static RoundManager;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    public TMP_Text addMoneyText;
    public TMP_Text removeMoneyText;

    public int stepDelay = 300;

    byte[] studyField = { 0, 0, 1, 2, 3, 1, 0, 2, 3, 1, 2, 1, 3, 2, 0, 1, 3, 2, 1, 0, 0,
        0, 1, 2, 3, 1, 0, 2, 3, 1, 2, 1, 3, 2, 0, 1, 3, 2, 1, 0, 0,
        0, 1, 2, 3, 1, 0, 2, 3, 1, 2, 1, 3, 2, 0, 1, 3, 2, 1, 0, 0,
        0, 1, 2, 3, 1, 0, 2, 3, 1, 2, 1, 3, 2, 0, 1, 3, 2, 1, 0};
    byte[] jobFieldPositive = { 16, 38 };
    byte[] jobFieldNegative = { 9, 25, 41, 57 };
    byte[] jobFieldSelary = { 8, 24, 40, 56 };
    byte[,] businessField =
    {
     { 0,0,0,1,0,0,4,0,0,0,1,0,0 },
     { 0,0,0,0,0,0,0,0,1,0,0,0,0 },
     { 0,0,2,0,0,1,0,0,0,0,2,0,0 },
     { 0,0,0,0,0,0,2,0,0,0,0,0,1 },
     { 0,0,0,0,0,0,1,0,0,0,0,0,0 },
     { 0,1,0,0,0,0,0,0,0,0,1,0,0 },
     { 0,0,0,0,1,0,3,0,1,0,0,0,0 },
     { 0,0,1,0,0,0,0,0,0,0,0,0,0 },
     { 0,0,0,0,0,0,1,0,0,0,0,0,1 },
     { 0,0,0,0,0,0,2,0,0,0,0,0,0 },
     { 0,0,2,0,0,0,0,1,0,0,2,0,0 },
     { 0,0,0,0,0,0,0,0,0,0,0,0,0 },
     { 0,0,0,0,1,0,0,0,0,1,0,0,0 },
    };
    string[] graduationTexts =
   {
        "\nVysvìdèení\n_______________________\n\nDržitel tohoto dokumentu splnil podmínky pro dokonèení základní školy, tudíž má dokonèené\n\nZákladní\nVzdìlání\n\n\n\n\n\n_______________________",
        "\nMaturitní\nVysvìdšení\n_______________________\n\nDržitel tohoto dokumentu úspìšnì absolvoval maturitní zkoužku, tudíž má dokonèené\n\nStøedoškolské\nVzdìlání\n\n\n\n\n\n_______________________",
        "\nVysokoškolský\nDiplom\n_______________________\n\nDržitel tohoto dokumentu úspìšnì vykonal státní závìreèné zkoušky a obhájil diplomovou práci, tudíž má dokonèené\n\nVysokoškolské\nVzdìlání\n\n\n\n_______________________",
    };

    float moveDistance = 0.42f ;
    public enum direction { up, down, right, left, none };

    public static bool arrowClidked = false;
    public static direction moveDirection;
    public static direction lastDirection = direction.none;

    public DiceThrower DiceThrower;
    public CardManager cards;

    GameObject[] coins = new GameObject[5];
    public GameObject coinPrefab;
    public GameObject corner;

    private void Awake()
    {
        int coinCount = coins.Length;
        while (coinCount != 0) 
        {
            int x = Random.Range(0, 12);
            int y = Random.Range(0, 12);
            bool canBePlaced = true;
            for (int i = x - 2; i <= x + 2; i++)
            {
                for (int j = y - 2; j <= y + 2; j++)
                {
                    if (i >= 0 && j >= 0 && i < 12 && j <= 12)
                        if (businessField[j, i] > 4)
                            canBePlaced = false;
                }
            }
            if (canBePlaced && businessField[y, x] == 0)
            {
                businessField[y, x] =(byte) (4 + coinCount);
                coins[coinCount-1] = Instantiate(coinPrefab, new Vector3(x * moveDistance + corner.transform.position.x, 0.02f, y * moveDistance + corner.transform.position.z), Quaternion.Euler(90,0,0));
                //Debug.Log(x + ", " + y);
                coinCount--;
            }  
        }


    }

    public async void studyMove(int moveCount)
    {
        arrowClidked = false;
        Figure figure = players[playerOnTurn].Fg[figureOnTurn];
        for (int i = 0; i < moveCount; i++)
        {
            if (figure.positionX < 20)
                MoveLeft(figure);
            else if (figure.positionX < 40)
                MoveUp(figure);
            else if (figure.positionX < 60)
                MoveRight(figure);
            else if (figure.positionX < 80)
                MoveDown(figure);

            figure.positionX++;
            if (figure.positionX == 80) figure.positionX = 0;
            await Task.Delay(stepDelay);
        }
        if (studyField[figure.positionX] == (figure.education + 1))
        {
            figure.education++;
            if (figure.education == 1)
                cards.CreateCard(cards.Yellow, cards.ZsTop.transform.position, true, graduationTexts[0], 0);
            else if(figure.education == 2)
                cards.CreateCard(cards.Orange, cards.SsTop.transform.position, true, graduationTexts[1], 0);
            else if(figure.education == 3)
                cards.CreateCard(cards.Red, cards.VsTop.transform.position, true, graduationTexts[2], 0);
        }
        diceToPlay--;
        diceIsSelected = false;
        if (diceToPlay == 0) phase = phaseType.end;
        else phase = phaseType.chooseDirection;
    }
    public async void jobMove(int moveCount)
    {
        arrowClidked = false;
        Figure figure = players[playerOnTurn].Fg[figureOnTurn];
        for (int i = 0; i < moveCount; i++)
        {
            if (figure.positionX < 8 || figure.positionX > 55)
                MoveLeft(figure);
            else if (figure.positionX < 24)
                MoveUp(figure);
            else if (figure.positionX < 40)
                MoveRight(figure);
            else if (figure.positionX < 56)
                MoveDown(figure);

            figure.positionX++;
            for (int j = 0; j < jobFieldSelary.Length; j++)
                if (figure.positionX == jobFieldSelary[j])
                {
                    MoneyPopup(figure, 20000);
                    figure.money += 20000;
                }
                     

            if (figure.positionX == 63) figure.positionX = 0;
            await Task.Delay(stepDelay);
        }
        for (int i = 0;i < jobFieldNegative.Length;i++)
            if (figure.positionX == jobFieldNegative[i])
                cards.DrawNegativeCard(0); // draw negative card todo
        for (int i = 0; i < jobFieldPositive.Length; i++)
            if (figure.positionX == jobFieldPositive[i])
                cards.DrawPositiveCard(); // draw positive card todo

        diceToPlay =0;
        diceIsSelected = false;

        if (diceToPlay == 0) phase = phaseType.end;
        else phase = phaseType.chooseDirection;
    }
    public async     Task
businessMove(int moveCount)
    {
        arrowClidked = false;
        Figure figure = players[playerOnTurn].Fg[figureOnTurn];
        for (int i = 0;i < moveCount; i++)
        {
            switch (moveDirection)
            {
                case direction.up:
                    MoveUp(figure);
                    figure.positionY++;
                    lastDirection = direction.up;
                    break;
                case direction.down:
                    MoveDown(figure);
                    figure.positionY--;
                    lastDirection = direction.down;
                    break;
                case direction.right:
                    MoveRight(figure);
                    figure.positionX++;
                    lastDirection = direction.right;
                    break;
                case direction.left:
                    MoveLeft(figure);
                    figure.positionX--;
                    lastDirection = direction.left;
                    break;
            }
            //Debug.Log(businessField[figure.positionY, figure.positionX]);
            if (businessField[figure.positionY, figure.positionX] == 1)
                cards.DrawNegativeCard(0);     // draw negative card todo                

            await Task.Delay(stepDelay);
        }
        diceToPlay--;
        diceIsSelected = false;

        List<Figure> playersOnBox = new List<Figure>();

        if (businessField[figure.positionY, figure.positionX] > 4)
        {
            Destroy(coins[businessField[figure.positionY, figure.positionX]-5]);
            figure.money += 30000;
            MoneyPopup(figure, 30000);
            businessField[figure.positionY, figure.positionX] = 0;
            Debug.Log("30k coin");
        }
        for (int j = 0; j < Lobby.playerCount; j++)
            for (int k = 0; k < 2; k++)
            if (players[j].Fg[k].status == Figure.statusType.business && !players[j].Fg[k].studying && players[j].Fg[k].positionX == figure.positionX && players[j].Fg[k].positionY == figure.positionY)
                playersOnBox.Add(players[j].Fg[k]);
        if (playersOnBox.Count > 1 && businessField[figure.positionY, figure.positionX] != 4)
        {
            int edu = 0;
            foreach (Figure p in playersOnBox) 
                if (p.education > edu)
                    edu = p.education;
            int moneyAmount;
            if (diceToPlay == 0)
            {            //VIP obchod
                //Debug.Log("c " + playersOnBox.Count);
                DiceThrower.amoutOfDice = playersOnBox.Count;
                DiceThrower.RollDice();

                for (int i = 0; i < DiceThrower.spawnedDices.Length; i++)
                    if (DiceThrower.spawnedDices[i] != null)
                    {
                    EventTrigger et = DiceThrower.spawnedDices[i].GetComponent<EventTrigger>();
                    et.enabled = false;
                    Debug.Log(i);
                    }

                int diceValue = 0;
                for (int i = 0; i < DiceThrower.spawnedDices.Length; i++)
                    if (DiceThrower.spawnedDices[i] != null)
                    {
                        int topFace = DiceThrower.spawnedDices[i].GetComponent<Dice>().value;
                        diceValue += topFace;
                        if (topFace == 0)
                        {
                            await Task.Delay(100);
                            i--;
                        }
                        Debug.Log(i);
                    }

                await Task.Delay(5000);
                for (int i = 0; i < DiceThrower.spawnedDices.Length; i++)
                {
                    Destroy(DiceThrower.spawnedDices[i]);
                    Debug.Log("x"+i);
                }
                    

                if (diceValue < 5) {
                    if (edu == 1) moneyAmount = 5000;
                    else if (edu == 2) moneyAmount = 10000;
                    else moneyAmount = 20000;
                }
                else if (diceValue < 7) {
                    if (edu == 1) moneyAmount = 10000;
                    else if (edu == 2) moneyAmount = 15000;
                    else moneyAmount = 25000;
                }
                else if (diceValue < 9) {
                    if (edu == 1) moneyAmount = 15000;
                    else if (edu == 2) moneyAmount = 20000;
                    else moneyAmount = 30000;
                }
                else if (diceValue < 11) {
                    if (edu == 1) moneyAmount = 20000;
                    else if (edu == 2) moneyAmount = 25000;
                    else moneyAmount = 35000;
                }
                else {
                    if (edu == 1) moneyAmount = 25000;
                    else if (edu == 2) moneyAmount = 30000;
                    else moneyAmount = 40000;
                }  
            }
            else
            {           //bìžný obchod
                if (edu == 1) moneyAmount = 5000;
                else if (edu == 2) moneyAmount = 10000;
                else moneyAmount = 15000;
            }
            if (businessField[figure.positionY, figure.positionX] > 1 && businessField[figure.positionY, figure.positionX] < 4) moneyAmount *= businessField[figure.positionY, figure.positionX];

            foreach (Figure p in playersOnBox)
                    p.money += moneyAmount;
            MoneyPopup(figure, moneyAmount);
            Debug.Log("vip " + moneyAmount);
        }



        if (!players[playerOnTurn].isAi)
            if (diceToPlay == 0)
                phase = phaseType.end;

            else phase = phaseType.chooseDirection;
    }
    void MoveLeft(Figure figure)
    {
            figure.transform.position = new Vector3(figure.transform.position.x - moveDistance, figure.transform.position.y, figure.transform.position.z);
    }
    void MoveRight(Figure figure)
    {
            figure.transform.position = new Vector3(figure.transform.position.x + moveDistance, figure.transform.position.y, figure.transform.position.z);
    }
    void MoveUp(Figure figure)
    {
            figure.transform.position = new Vector3(figure.transform.position.x, figure.transform.position.y, figure.transform.position.z + moveDistance);
    }
    void MoveDown(Figure figure)
    {
            figure.transform.position = new Vector3(figure.transform.position.x, figure.transform.position.y, figure.transform.position.z - moveDistance);
    }
    public void MoneyPopup(Figure figure, int money)
    {
        if (money > 0)
        {
            addMoneyText.text = "+" + money;
            Instantiate(addMoneyText, figure.GetComponentInChildren<Canvas>().transform);
        }   
        else
        {
            removeMoneyText.text = "" + money;
            Instantiate(removeMoneyText, figure.GetComponentInChildren<Canvas>().transform);
        }
            


    }
}
