using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{
    public Sprite[] colorImg;
    public string[] colorText = { "èervená", "modrá", "zelená", "žlutá", "oranžová", "èerná", "bílá", "rùžová", "fialová", "tyrkysová", "hnìdá", "šedá"};

    public TMP_Dropdown[] dropdowns = new TMP_Dropdown[8];
    
    // Start is called before the first frame update
    void Awake()
    {

        for (int i = 0;i < 8; i++)
        {
            dropdowns[i] = transform.GetChild(i).GetComponent<TMP_Dropdown>();
            dropdowns[i].options.Clear();
            
        }

        for (int k = 0; k < colorText.Length; k++)
        {
            for (int j = 0; j < 8; j++)
            {
                dropdowns[j].options.Add(new TMP_Dropdown.OptionData() { text = colorText[k], image = colorImg[k] });
                dropdowns[j].value = j;
                
            }         
        }
       
    }
   
    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i < 8; i++)
        {
            if (i>= Lobby.playerCount)
                dropdowns[i].gameObject.SetActive(false);
            else
                dropdowns[i].gameObject.SetActive(true);
        }
        

    }
}
