using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Material[] materials;
    

    public void SetColor(Renderer figure, int colorNumber)
    {
        figure.material = materials[colorNumber];
    }

}
