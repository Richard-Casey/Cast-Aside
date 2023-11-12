using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseSymbolImage;
    [SerializeField] Image background;
    [SerializeField] Color backgroundColor;

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject ConfirmQuit;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider Master;
    [SerializeField] Slider SFX;
    [SerializeField] Slider Music;

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Toggle FullscreenToggle;
    // Start is called before the first frame update
    public void Start()
    {
        GetScreenRes();
        InputManager.onPausePress.AddListener(onPause);

        Master.onValueChanged.AddListener(SetMasterVolume);
        SFX.onValueChanged.AddListener(SetMusicVolume);
        Music.onValueChanged.AddListener(SetSFXVolume);

        dropdown.onValueChanged.AddListener(ChangeResolution);

        Master.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MasterVolume);
        SFX.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume);
        Music.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume);
    }

    public void OnDisable()
    {
        InputManager.onPausePress.RemoveListener(onPause);
        Master.onValueChanged.RemoveListener(SetMasterVolume);
        SFX.onValueChanged.RemoveListener(SetMusicVolume);
        Music.onValueChanged.RemoveListener(SetSFXVolume);
        dropdown.onValueChanged.RemoveListener(ChangeResolution);
    }

    public void SetMasterVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting Master Volume: {volume}");
        if (volume == 0)
        {
            mixer.SetFloat("MasterVolume", -80); // Mute the volume
        }
        else
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MasterVolume, volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting Music Volume: {volume}");
        if (volume == 0)
        {
            mixer.SetFloat("MusicVolume", -80); // Mute the volume
        }
        else
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting SFX Volume: {volume}");
        if (volume == 0)
        {
            mixer.SetFloat("SFXVolume", -80); // Mute the volume
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SFXVolume, volume);
        PlayerPrefs.Save();
    }

    //Dictionary<string,Vector2> Resolutio
    public void ChangeResolution(int index)
    {
        string OptionAsString = dropdown.options[index].text;
        string[] Components = OptionAsString.Split("x");

        if(!int.TryParse(Components[0], out int width)) return;
        if(!int.TryParse(Components[1], out int height)) return;
        Screen.SetResolution(width,height, FullscreenToggle.isOn);
        Debug.Log(Screen.currentResolution);
    }


    public void GetScreenRes()
    {
        List<TMP_Dropdown.OptionData> Options = new List<TMP_Dropdown.OptionData>();
        foreach (var res in Screen.resolutions)
        {
            Options.Add(new TMP_Dropdown.OptionData(res.width.ToString() + "x" + res.height.ToString()));
        }
        dropdown.options = Options;
    }

    public void ShowMainMenu()
    {
        RectTransform mainMenuRectTransform = MainMenu.GetComponent<RectTransform>();

        Vector3 InitalPos = mainMenuRectTransform.transform.position;
        MainMenu.SetActive(true);

    }




    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    public void ShowOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void ShowConfirmQuit()
    {
        ConfirmQuit.SetActive(true);
    }


    public void HideMainMenu()
    {
        MainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void HideOptions()
    {
        OptionsMenu.SetActive(false);
    }

    public void HideConfirmQuit()
    {
        ConfirmQuit.SetActive(false);
    }

    public void onUnpause()
    {
        //Invert the function called when unpausing
        InputManager.onPausePress.AddListener(onPause);
        InputManager.onPausePress.RemoveListener(onUnpause);


        //Disable All Menus
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ConfirmQuit.SetActive(false);

        //Hide pause symbol
        pauseSymbolImage.SetActive(false);

        //Turn Time scale back
        Time.timeScale = 1f;

        //Change Color and then revert time scale
        background.DOColor(new Color(0, 0, 0, 0), 1f);

    }

    public void onPause()
    {
        //Invert the function called when unpausing
        InputManager.onPausePress.RemoveListener(onPause);
        InputManager.onPausePress.AddListener(onUnpause);


        //Show pause symbol
        pauseSymbolImage.SetActive(true);


        //Change Color and then open the menu
        background.DOColor(backgroundColor, 1f).OnComplete(() => { 
            ShowMainMenu(); 
            //Stop the game running
            Time.timeScale = 0f;
        });



    }

}
