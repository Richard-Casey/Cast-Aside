using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionControl : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        string CurrentRes = Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString();


        

        List<TMP_Dropdown.OptionData> Options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            Options.Add(new TMP_Dropdown.OptionData(option));
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.options = Options;

        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == CurrentRes)
            {
                resolutionDropdown.value = i; 
                return;

            }
        }

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


}