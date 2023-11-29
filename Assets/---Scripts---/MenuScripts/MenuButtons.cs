using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public InputActionAsset pia;

    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject customisationMenu;
    public GameObject controlsMenu;

    public AudioSource buttonAudioSource;

    private void Awake()
    {
        buttonAudioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
    }

    public void StartButton()
    {
        Debug.Log("Start Button pressed - loading game");
        SceneManager.LoadSceneAsync("TransitionScene");
        buttonAudioSource.Play();
    }

    public void ResetProgress()
    {
        bool end = false;
        int i = 1;
        while (end == false)
        {
            int value = PlayerPrefs.GetInt("Prefab_" + i.ToString(), -1);
            if (value != -1)
            {
                PlayerPrefs.SetInt("Prefab_" + i.ToString(),0);
                i++;
                continue;
            }
            end = true;
            return;
        }
    }

    public void ExitButton()
    {
        Debug.Log("Exiting Game");
        buttonAudioSource.Play();
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        buttonAudioSource.Play();
        mainMenu.SetActive(false);
        controlsMenu.SetActive(false);
        optionsMenu.SetActive(true);
        
    }

    public void PlayerButtonPress()
    {
        buttonAudioSource.Play();
    }

    public void CloseOptionsMenu()
    {
        
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        buttonAudioSource.Play();
    }

    public void OpenCustomisationMenu()
    {
        buttonAudioSource.Play();
        mainMenu.SetActive(false);
        controlsMenu.SetActive(false);
        customisationMenu.SetActive(true);
        
    }

    public void CloseCustomisationMenu()
    {
        
        mainMenu.SetActive(true);
        customisationMenu.SetActive(false);
        controlsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        buttonAudioSource.Play();
    }

    public void OpenControlsCanvas()
    {
        pia.Disable();
        buttonAudioSource.Play();
        mainMenu.SetActive(false);
        customisationMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true);

    }

    public void CloseControlsCanvas()
    {
        pia.Enable();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        customisationMenu.SetActive(false);
        buttonAudioSource.Play();
    }

}