using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoundManager;

public class arrow : MonoBehaviour
{
    public Movement.direction direction;

    public void onArrowClick()
    {
        Movement.moveDirection = direction;
        Movement.arrowClidked = true;
        phase = phaseType.moving;
        if (!(players[playerOnTurn].Fg[figureOnTurn].status == Figure.statusType.job && !players[playerOnTurn].Fg[figureOnTurn].studying))
        Destroy(selectedDice.gameObject);
    }
}
