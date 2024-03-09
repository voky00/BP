using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RoundManager;

public class CardManager : MonoBehaviour
{
    public GameObject ZsTop;
    public GameObject SsTop;
    public GameObject VsTop;
    public GameObject GoodTop;
    public GameObject BadTop;

    public Material Yellow;
    public Material Orange;
    public Material Red;
    public Material Blue;

    public Card cardPrefab;

    public Movement movement;


    int[] positiveValues = { 30000, 20000, 10000, 5000 };
    int[] negativeValues = { -30000, -20000, -10000, -5000 };
    string[] positiveCardTexts = 
    {
        "__________________________________________\n\n\nVyhr·l jsi ve sportce 30 000 KË.\n\n\n\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nDostal jsi mimo¯·dnÈ prÈmie ve v˝öi 20 000 KË.\n\n\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nSt·t ti dal mimo¯·dnou podporu za stÌûenÈ pracovnÌ podmÌnky ve v˝öi 10 000 KË.\n\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nNa zemi jsi naöel 5 000 KË.\n\n\n\n\n\n\n__________________________________________",
    };
    string[] negativeCardTexts =
    {
        "__________________________________________\n\n\nRozbilo se ti auto musÌö zaplatit 30 000 KË za oplavu.\n\n\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nRozbila se ti ledniËka zaplaù 20 000 KË oplavu.\n\n\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nZtratil jsi penÏûenku ve kterÈ jsi mÏl 10 000 KË, potÈ jsi jÌ naöel ve str·tech a n·lezech ale penÌze byly fuË.\n\n\n\n__________________________________________",
        "__________________________________________\n\n\nRozbili se ti br˝le musÌö zaplatit 5 000 KË za novÈ.\n\n\n\n\n\n__________________________________________",
    };

   

    public void DrawPositiveCard()
    {
        if (players[playerOnTurn].isAi) return;
        Figure fg = players[playerOnTurn].Fg[figureOnTurn];

        int index = UnityEngine.Random.Range(0, positiveValues.Length);
        fg.money += positiveValues[index];

        movement.MoneyPopup(fg, positiveValues[index]);
        Debug.Log(positiveCardTexts[index]);

        CreateCard(Blue, GoodTop.transform.position, false, positiveCardTexts[index], 0);
    }
    public void DrawNegativeCard(float offset)
    {
        if (players[playerOnTurn].isAi) return;
        Figure fg = players[playerOnTurn].Fg[figureOnTurn];

        
        int index = UnityEngine.Random.Range(0, negativeValues.Length);
        fg.money += negativeValues[index];

        movement.MoneyPopup(fg, negativeValues[index]);
        Debug.Log(negativeCardTexts[index]);

        CreateCard(Blue, BadTop.transform.position, false, negativeCardTexts[index], offset);
    }
    public void CreateCard(Material color, Vector3 v3, bool isVertical, string text, float offset)
    {
        
        Card card = Instantiate(cardPrefab, v3, transform.rotation);
        // card.GetComponentInChildren<RectTransform>().transform;  gameObject.GetComponent<RectTransform> ();
        Material[] colors = new Material[3];
        colors[0] = color;
        colors[1] = color;
        colors[2] = color;

        card.offSet = offset;
        card.GetComponent<MeshRenderer>().materials = colors;
        card.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        card.transform.rotation = Quaternion.Euler(90, 0, 0);

        if (isVertical) 
        {
            card.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0.6f, 1.1f);
            card.transform.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);
            card.transform.rotation = Quaternion.Euler(270, 0, 0);
            card.vertical = true;
        }
        else
        {
            card.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1.1f, 0.6f);
            card.transform.rotation = Quaternion.Euler(270, 0, 90);
            card.vertical = false;
        }
        

    }
    private void Awake()
    {
        //CreateCard(Red, VsTop.transform.position, true, graduationTexts[2]);
    }
}
