using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static RoundManager;
public class Lobby : MonoBehaviour
{
    public static int roundCount = 10;
    public static int playerCount = 2;

    public static string[] playerNames = new string[8];
    public static int[] playerColors = new int[8];

    public TMP_Dropdown playerCountDrop;
    public TMP_Dropdown roundCountDrop;
    public TextInput namesInput;
    public Dropdown colorsInput;
    public Toggles aiToggles;

    public void PlayerCountChange()
    {
        playerCount = (playerCountDrop.value + 2);  
    }
    public void RoundCountChange()
    {
        roundCount = (roundCountDrop.value * 5 + 10);
    }
    public void GameStart()
    {
        for (int i = 0; i < 8; i++)
            Destroy(players[i]);

        //RoundManager.players = new Player[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            players[i] = new Player();
            players[i].color = colorsInput.dropdowns[i].value;
            players[i].playerName = namesInput.textInputs[i].text;
        }
        for (int i = 0; i < playerCount-1; i++)
            players[i+1].isAi = aiToggles.toggles[i].isOn;

        round = 0;
        playerOnTurn = 0;
        figureOnTurn = 0;
        phase = phaseType.start;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
    

