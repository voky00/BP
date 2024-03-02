using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class Dice : MonoBehaviour
{
    public Transform[] diceFaces;
    public Rigidbody rb;
    public Animator animator;

    int topFace;
    public int value = 0;

    private int diceIndex = -1;

    public bool hasStoppedRolling;
    bool delayFinished;

    public static UnityAction<int, int> OnDiceResult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!delayFinished) return;

        if (!hasStoppedRolling && rb.velocity.sqrMagnitude == 0f) 
        {
            hasStoppedRolling = true;
            GetNumberOnTopFace();
            if (RoundManager.players[RoundManager.playerOnTurn].Fg[RoundManager.figureOnTurn].status == Figure.statusType.job)
                RoundManager.diceIsSelected = true;
        }
    }

    public void selectDice(Dice dice)
    {
        RoundManager.diceIsSelected = true;
        if (RoundManager.selectedDice != null)
        RoundManager.selectedDice.GetComponent<Animator>().SetBool("Selected", false);
        animator.SetBool("Selected", true);
        RoundManager.selectedDice = dice;
        
    }
    private int GetNumberOnTopFace()
    {
        if(diceFaces == null) return -1;

        
        var lastYPosition = diceFaces[0].position.y;

        for(int i = 1; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }

        //Debug.Log($"Dice result {topFace + 1}");
        animator.SetBool("Idle", true);

        OnDiceResult?.Invoke(diceIndex, topFace +1);
        value = topFace +1;
        return topFace + 1;
    }

    internal void RollDice(float throwForce, float rollForce, int i)
    {
        diceIndex = i;
        var randomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);

        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);

        rb.AddTorque(new Vector3 (randX, randY, randZ) * (rollForce + randomVariance), ForceMode.Impulse);

        DelayResult();
    }

    private async void DelayResult()
    {
        await Task.Delay(1000);
        delayFinished = true;
    }
}
