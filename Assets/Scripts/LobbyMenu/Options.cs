using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class Options : MonoBehaviour
{
    Resolution[] resolutions = new Resolution[4];
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public TMP_Dropdown graphicDropdown;
    
    static public int currentResolutionIndex = 0;
    void Awake()
    {
        resolutions[0].width = 1920;
        resolutions[0].height = 1080;
        resolutions[0].refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 };

        resolutions[1].width = 1440;
        resolutions[1].height = 900;
        resolutions[1].refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 };

        resolutions[2].width = 1366; 
        resolutions[2].height = 768;
        resolutions[2].refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 };

        resolutions[3].width = 1280;
        resolutions[3].height = 720;
        resolutions[3].refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 };

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);  
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        SetResolution(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;

        graphicDropdown.value = QualitySettings.GetQualityLevel();
    }
    public void SetGraphic(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }
    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
