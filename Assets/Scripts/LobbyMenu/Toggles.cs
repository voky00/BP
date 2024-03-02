using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toggles : MonoBehaviour
{
    public Toggle[] toggles = new Toggle[7];


    void Start()
    {
        for (int i = 0; i < 7; i++)
            toggles[i] = transform.GetChild(i).GetComponent<Toggle>();
    }
    void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i >= Lobby.playerCount-1)
                toggles[i].gameObject.SetActive(false);
            else
                toggles[i].gameObject.SetActive(true);
        }
    }
}


