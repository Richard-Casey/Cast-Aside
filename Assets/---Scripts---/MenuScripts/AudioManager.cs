using UnityEngine;
using UnityEngine.Audio;

public static class PlayerPrefsKeys
{
    public const string MasterVolume = "MasterVolume";
    public const string MusicVolume = "MusicVolume";
    public const string SFXVolume = "SFXVolume";
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mainAudioMixer;

    public AudioClip buttonClickSound;
    public AudioClip hoverSound;
    public AudioClip menuMusic;
    public AudioClip hoverClick;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        LoadVolumeSettings();
        Debug.Log("LoadVolumeSettings called on startup");
    }

    public void SetMasterVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting Master Volume: {volume}");
        if (volume == 0)
        {
            mainAudioMixer.SetFloat("MasterVolume", -80); // Mute the volume
        }
        else
        {
            mainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MasterVolume, volume);
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting Music Volume: {volume}");
        if (volume == 0)
        {
            mainAudioMixer.SetFloat("MusicVolume", -80); // Mute the volume
        }
        else
        {
            mainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, volume);
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log($"[AudioManager] Setting SFX Volume: {volume}");
        if (volume == 0)
        {
            mainAudioMixer.SetFloat("SFXVolume", -80); // Mute the volume
        }
        else
        {
            mainAudioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SFXVolume, volume);
        SaveVolumeSettings();
    }


    public void LoadVolumeSettings()
    {
        // Use 0.75f as the default value if the key does not exist
        float masterVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MasterVolume, 0.75f);
        float musicVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, 0.75f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        Debug.Log($"[AudioManager] Loaded Volume Settings - Master: {masterVolume}, Music: {musicVolume}, SFX: {sfxVolume}");
    }


    public void SaveVolumeSettings()
    {
        PlayerPrefs.Save();
    }

    public void PlayUIHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayUIClick()
    {
        audioSource.PlayOneShot(hoverClick);
    }

    public void PlayMenuMusic()
    {
        audioSource.PlayOneShot(menuMusic);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
