using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInput : MonoBehaviour
{
    public TMP_InputField[] textInputs = new TMP_InputField[8];
    

    void Start()
    {
        for (int i = 0; i < 8; i++)
            textInputs[i] = transform.GetChild(i).GetComponent<TMP_InputField>();
    }
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i >= Lobby.playerCount)
                textInputs[i].gameObject.SetActive(false);
            else
                textInputs[i].gameObject.SetActive(true);
        }
    }
}
