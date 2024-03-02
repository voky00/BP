using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RoundManager;

public class EndScreen : MonoBehaviour
{
    public GameObject name;
    public GameObject jobMoney;
    public GameObject businessMoney;

    public TMP_Text jobWinner;
    public TMP_Text businessWinner;
    void Update()
    {
        
    }
    public void writeEndInfo()
    {
        long jobWinValue = -999999999, businessWinValue = -999999999;
        TMP_Text[] names = new TMP_Text[8];
        TMP_Text[] jobMoneys = new TMP_Text[8];
        TMP_Text[] businessMoneys = new TMP_Text[8];

        for (int i = 0; i < Lobby.playerCount; i++)
        {
            names[i] = name.transform.GetChild(i).GetComponent<TMP_Text>();
            jobMoneys[i] = jobMoney.transform.GetChild(i).GetComponent<TMP_Text>();
            businessMoneys[i] = businessMoney.transform.GetChild(i).GetComponent<TMP_Text>();

            names[i].gameObject.SetActive(true);
            jobMoneys[i].gameObject.SetActive(true);
            businessMoneys[i].gameObject.SetActive(true);

            names[i].text = players[i].playerName;

            Figure fg1 = players[i].Fg[0];
            Figure fg2 = players[i].Fg[1];

            if (players[i].isAi)
            {
                jobMoneys[i].text = "-";
                businessMoneys[i].text = "-";
                names[i].text += " (bot)";
                continue;
            }
            if (fg2.status == Figure.statusType.business)
            {
                if (fg1.money > jobWinValue)
                {
                    jobWinner.text = "Nejlepší zamìstnanec\n" + players[i].playerName;
                    jobWinValue = fg1.money;
                }
                else if (fg1.money == jobWinValue)
                    jobWinner.text += " a " + players[i].playerName;

                if (fg2.money > businessWinValue)
                {
                    businessWinner.text = "Nejlepší podnikatel\n" + players[i].playerName;
                    businessWinValue = fg2.money;
                }
                else if (fg2.money == businessWinValue)
                    businessWinner.text += " a " + players[i].playerName;

                jobMoneys[i].text = fg1.money.ToString();
                businessMoneys[i].text = fg2.money.ToString();
            }
            else
            {
                if (fg2.money > jobWinValue)
                {
                    jobWinner.text = "Nejlepší zamìstnanec\n" + players[i].playerName;
                    jobWinValue = fg2.money;
                }
                else if (fg2.money == jobWinValue)
                    jobWinner.text += " a " + players[i].playerName;

                if (fg1.money > businessWinValue)
                {
                    businessWinner.text = "Nejlepší podnikatel\n" + players[i].playerName;
                    businessWinValue = fg1.money;
                }
                else if (fg1.money == businessWinValue)
                    businessWinner.text += " a " + players[i].playerName;

                jobMoneys[i].text = fg2.money.ToString();
                businessMoneys[i].text = fg1.money.ToString();
            }

        }
    }
}
