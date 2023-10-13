using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;  // Reference to the Audio Mixer asset
    public string masterVolumeParameterName = "Master";
    public string musicVolumeParameterName = "Music";
    public string sfxVolumeParameterName = "SFX";


    public void SetMasterVolume(float volume)
    {
        Debug.Log("Setting Master Volume to " + volume);
        audioMixer.SetFloat(masterVolumeParameterName, volume);
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("Setting Music Volume to " + volume);
        audioMixer.SetFloat(musicVolumeParameterName, volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("Setting SFX Volume to " + volume);
        audioMixer.SetFloat(sfxVolumeParameterName, volume);
    }
}