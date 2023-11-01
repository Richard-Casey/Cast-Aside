using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip buttonClickSound;
    public AudioClip hoverSound;
    public AudioClip menuMusic;
    public AudioClip hoverClick;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioManager exists
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