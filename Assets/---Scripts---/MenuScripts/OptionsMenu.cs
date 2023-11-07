using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private AudioManager audioManager;

    private void Awake()
    {
        // Get the AudioManager instance
        audioManager = AudioManager.instance;
    }

    private void OnEnable()
    {
        Debug.Log("OptionsMenu OnEnable called");
        InitialiseSliders();
        masterVolumeSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);
    }

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    private void InitialiseSliders()
    {
        // Set the sliders to the saved values or the default if they don't exist
        masterVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MasterVolume, 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, 0.75f);

        // Add listeners to the sliders that call the AudioManager's methods
        masterVolumeSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);

        Debug.Log($"[OptionsMenu] Initialising Sliders - Master: {masterVolumeSlider.value}, Music: {musicVolumeSlider.value}, SFX: {sfxVolumeSlider.value}");
    }

    private void LoadVolumeSettings()
    {
        // Call the AudioManager's LoadVolumeSettings method
        audioManager.LoadVolumeSettings();
    }

    public void OpenOptionsMenu()
    {
        // Set the sliders to the saved values or the default if they don't exist
        masterVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MasterVolume, 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, 0.75f);

        // Update the AudioManager's volume settings based on the sliders' values
        audioManager.SetMasterVolume(masterVolumeSlider.value);
        audioManager.SetMusicVolume(musicVolumeSlider.value);
        audioManager.SetSFXVolume(sfxVolumeSlider.value);

        Debug.Log($"[OptionsMenu] Opening Options Menu - Master: {masterVolumeSlider.value}, Music: {musicVolumeSlider.value}, SFX: {sfxVolumeSlider.value}");
    }
}
