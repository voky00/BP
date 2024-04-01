using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static RoundManager;

public class Timer : MonoBehaviour
{
    public static float time = 60;
    public static bool timerActive = false;
    public TMP_Text timeCounterText;
    public GameObject TimeCounter;
    public RoundManager roundManager;
    public CardManager cards;
    private void Update()
    {
        if (timerActive)
        {
            time -= Time.deltaTime;
            timeCounterText.SetText("Zbývající èas: " + Mathf.Round(time) + "s");
            if (time <= 0) TimerPenality();
            if (diceToPlay == 0)
            {
                timerActive = false;
                TimeCounter.SetActive(false);
            }
        }
    }
    public void SetTimer()
    {
        if (players[playerOnTurn].Fg[figureOnTurn].status == Figure.statusType.business 
            && !players[playerOnTurn].Fg[figureOnTurn].studying)
        {
            TimeCounter.SetActive(true);
            timerActive = true;
            time = 60;
        }  
    }
    public async void TimerPenality()
    {
        timerActive = false;
        cards.DrawNegativeCard(0);
        await Task.Delay(500);
        cards.DrawNegativeCard(0.1f);
        roundManager.MoveToBusiness();
        TimeCounter.SetActive(false);
        for (int i = 0; i < DiceThrower.spawnedDices.Length; i++)
            if (DiceThrower.spawnedDices[i] != null)
            {
                Destroy(DiceThrower.spawnedDices[i]);
            }
        phase = phaseType.end;
    }
}
