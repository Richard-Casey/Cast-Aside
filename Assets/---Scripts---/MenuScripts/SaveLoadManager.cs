using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Going to make this persistant across scenes to allow for save and load of settings from in-game options menu.
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Saving the settings
    public void SaveSettings(float masterVolume, float sfxVolume, float musicVolume, int resolutionIndex,
        bool isFullscreen, int qualityIndex)
    {
        //PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        //PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        //PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ?  1 : 0);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);

        PlayerPrefs.Save();
    }

    // Loading the settings
    public void LoadSettings()
    {
        //if (PlayerPrefs.HasKey("Master"))
        //{
        //    float masterVolume = PlayerPrefs.GetFloat("Master");
        //}

        //if (PlayerPrefs.HasKey("SFX"))
        //{
        //    float sfxVolume = PlayerPrefs.GetFloat("SFX");
        //    // Set your SFX volume here
        //}

        //if (PlayerPrefs.HasKey("Music"))
        //{
        //    float musicVolume = PlayerPrefs.GetFloat("Music");
        //    // Set your music volume here
        //}

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            // Set your resolution here
        }

        if (PlayerPrefs.HasKey("IsFullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen") == 1;
            // Set your fullscreen setting here
        }

        if (PlayerPrefs.HasKey("QualityIndex"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualityIndex");
            // Set your quality setting here
        }
    }

    //public void SaveMasterVolume(float masterVolume)
    //{
    //    PlayerPrefs.SetFloat("Master", masterVolume);
    //    PlayerPrefs.Save();
    //}

    //public void SaveMusicVolume(float musicVolume)
    //{
    //    PlayerPrefs.SetFloat("Music", musicVolume);
    //    PlayerPrefs.Save();
    //}

    //public void SaveSFXVolume(float sfxVolume)
    //{
    //    PlayerPrefs.SetFloat("SFX", sfxVolume);
    //    PlayerPrefs.Save();
    //}

    public void SaveResolutionIndex(int resolutionIndex)
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SaveQualityIndex(int qualityIndex)
    {
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SaveFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

}