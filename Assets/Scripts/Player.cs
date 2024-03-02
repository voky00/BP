using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName = "player";
    public Figure[] Fg = new Figure[2];
    public int color = 0;
    public bool isAi = false;
}
