using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{

    public static UnityEvent OnPause = new UnityEvent();
    public static UnityEvent OnUnpause = new UnityEvent();
    [SerializeField] GameObject pauseSymbolImage;
    [SerializeField] Image background;
    [SerializeField] Color backgroundColor;

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject ConfirmQuit;
    [SerializeField] GameObject ControlsMenu;

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
        notePlayerPrefs();
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

        string CurrentRes = Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString();

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == CurrentRes)
            {
                dropdown.value = i;
                return;
            }
        }
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
        OnUnpause?.Invoke();


        //restore player prefs on leave
        for (int i = 0; i < PlayerPrefsStore.Count; i++)
        {
            PlayerPrefs.SetInt("Prefab_" + i.ToString(), PlayerPrefsStore[i]);
        }

        SceneManager.LoadScene("Main Menu");
    }
    public void ShowOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void ShowControls()
    {
        ControlsMenu.SetActive(true);
    }

    public void HideControls()
    {
        ControlsMenu.SetActive(false);
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

    List<int> PlayerPrefsStore = new List<int>();
    public void notePlayerPrefs()
    {
        bool end = false;
        int i = 0;
        while (end == false)
        {
            int value = PlayerPrefs.GetInt("Prefab_" + i.ToString(),-1);
            if (value != -1)
            {
                PlayerPrefsStore.Add(value);
                i++;
                continue;
            }
            end = true;
            return;
        }
    } 

    public void onUnpause()
    {
        OnUnpause?.Invoke();

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
        OnPause?.Invoke();

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

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        
    }

    public void ReexD()
    {
        Debug.Log("Hellop");
        Debug.Log(" ");
    }

}
