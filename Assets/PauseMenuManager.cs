using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
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
    // Start is called before the first frame update
    public void Start()
    {
        InputManager.onPausePress.AddListener(onPause);

        Master.onValueChanged.AddListener(SetMasterVolume);
        SFX.onValueChanged.AddListener(SetMusicVolume);
        Music.onValueChanged.AddListener(SetSFXVolume);

    }

    public void OnDisable()
    {
        InputManager.onPausePress.RemoveListener(onPause);
        Master.onValueChanged.RemoveListener(SetMasterVolume);
        SFX.onValueChanged.RemoveListener(SetMusicVolume);
        Music.onValueChanged.RemoveListener(SetSFXVolume);

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


    public void ShowMainMenu()
    {
        RectTransform mainMenuRectTransform = MainMenu.GetComponent<RectTransform>();

        Vector3 InitalPos = mainMenuRectTransform.transform.position;
        MainMenu.SetActive(true);
        /*mainMenuRectTransform.transform.position = new Vector3(3000, MainMenu.transform.localPosition.y, MainMenu.transform.position.z);
        mainMenuRectTransform.transform.DOLocalMove(InitalPos, 1f);
        */

        /*
        mainMenuRectTransform.anchoredPosition = mainMenuRectTransform.anchoredPosition + (Vector2.up * Screen.currentResolution.height);

        DOTween.To(() => mainMenuRectTransform.anchoredPosition, x => mainMenuRectTransform.anchoredPosition = x, mainMenuRectTransform.anchoredPosition - (Vector2.up * Screen.currentResolution.height), 2);
        */

    }




    public void ReturnToMenu()
    {
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
