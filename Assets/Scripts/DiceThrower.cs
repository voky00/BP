using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    public Dice diceToThrow;
    public static int amoutOfDice = 3;
    public float throwForce = 5f;
    public float rollForce = 10f;

    public static GameObject[] spawnedDices = new GameObject[3];

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) RollDice();
    }
    public void RollDice()
    {
        if (diceToThrow == null) return;

        RoundManager.phase = RoundManager.phaseType.chooseDirection;



        for (int i = 0; i < amoutOfDice; i++)
        {
            Dice dice = Instantiate(diceToThrow, transform.position, transform.rotation);
            spawnedDices[i] = dice.gameObject;
            dice.RollDice(throwForce, rollForce, i);
        }
    }
}
