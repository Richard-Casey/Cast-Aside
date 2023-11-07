//using UnityEngine;
//using UnityEngine.Audio;

//public class VolumeControl : MonoBehaviour
//{
//    public AudioMixer audioMixer;  // Reference to the Audio Mixer asset
//    public string masterVolumeParameterName = "MasterVolume";
//    public string musicVolumeParameterName = "MusicVolume";
//    public string sfxVolumeParameterName = "SFXVolume";


//    public void SetMasterVolume(float volume)
//    {
//        Debug.Log($"[VolumeControl] Setting Master Volume: {volume}");
//        Debug.Log("Setting Master Volume to " + volume);
//        audioMixer.SetFloat(masterVolumeParameterName, volume);
//    }

//    public void SetMusicVolume(float volume)
//    {
//        Debug.Log($"[VolumeControl] Setting Music Volume: {volume}");
//        Debug.Log("Setting Music Volume to " + volume);
//        audioMixer.SetFloat(musicVolumeParameterName, volume);
//    }

//    public void SetSFXVolume(float volume)
//    {
//        Debug.Log($"[VolumeControl] Setting SFX Volume: {volume}");
//        Debug.Log("Setting SFX Volume to " + volume);
//        audioMixer.SetFloat(sfxVolumeParameterName, volume);
//    }
//}