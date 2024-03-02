using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StudySpawns : MonoBehaviour
{
    public static GameObject[] studySpawns = new GameObject[12];

    void Start()
    {
        for (int i = 0; i < studySpawns.Length; i++)
            studySpawns[i] = transform.GetChild(i).gameObject; 
    }
}
