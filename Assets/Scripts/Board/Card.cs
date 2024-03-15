using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject card;
    public bool vertical;
    public float offSet = 0;

    public bool movingIn=true;
    public bool movingOut=false;
    public bool rotating = true;

    private void FixedUpdate()
    {
        if (card.transform.position == new Vector3(-0.2f, 6f+ offSet, -2f))
            Destroy(card);

        if (card.transform.position == new Vector3(-0.2f, 6f+ offSet, 0.5f))
            movingIn=false;
       

        if (rotating)
            if (vertical)
            {
                card.transform.rotation = Quaternion.RotateTowards(card.transform.rotation, Quaternion.Euler(90, 0, 0), 2);
                if (card.transform.rotation == Quaternion.Euler(90, 0, 0))
                    rotating = false;
            }
            else
            {
                card.transform.rotation = Quaternion.RotateTowards(card.transform.rotation, Quaternion.Euler(90, 0, 90), 2);
                if (card.transform.rotation == Quaternion.Euler(90, 0, 90))
                    rotating = false;
            }

        if (movingIn)
            card.transform.position = Vector3.MoveTowards(card.transform.position, new Vector3(-0.2f, 6f + offSet, 0.5f), 0.1f);

        if(movingOut)
            card.transform.position = Vector3.MoveTowards(card.transform.position, new Vector3(-0.2f, 6 + offSet, -2), 0.05f);
    }

    public void PutOutCard()
    {
      
        if (movingOut)
        {
            movingIn = true;
            movingOut = false;
        }
        else
        {
            movingIn = false;
            movingOut = true;
        }
    }
}
