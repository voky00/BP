using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Figure : MonoBehaviour
{
    public enum statusType { none, job, business};
    public statusType status = statusType.none;
    public int education = 0;
    public bool studying = false;
    public int positionX;
    public int positionY;
    public long money = 0;
    public GameObject model;

  
}
