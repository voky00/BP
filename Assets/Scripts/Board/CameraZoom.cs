using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cm;
    public float speed = 0.2f;
    public bool cameraZoomIn = false;

    bool moving = false;

    public GameObject arrow;
    public GameObject listHover;
    // Start is called before the first frame update
    private void FixedUpdate()
    { 
        if (!moving) return;

        arrow.SetActive(false);
        listHover.SetActive(false);

        if (cameraZoomIn)
        {
            
            if (cm.transform.position == new Vector3(5, 2.5f, 4.4f))
            {
                arrow.SetActive(true);
                listHover.SetActive(false);
                moving = false;
            }
            else
                zoomIn();
        }
            
        else
        {
            if (cm.transform.position == new Vector3(0, 8, 0))
            {
                arrow.SetActive(false);
                listHover.SetActive(true);
                moving = false;
            } 
            else
                zoomOut();
        }
            
    }
    public void zoomIn()
    {
        cameraZoomIn = true;
        moving = true;
        cm.transform.position = Vector3.MoveTowards(cm.transform.position, new Vector3(5, 2.5f, 4.4f), speed);
    }

    public void zoomOut() 
    {
        cameraZoomIn = false;
        moving = true;
        cm.transform.position = Vector3.MoveTowards(cm.transform.position, new Vector3(0,8,0), speed);
    }
}
