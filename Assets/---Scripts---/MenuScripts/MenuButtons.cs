using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject customisationMenu;

    public AudioSource buttonAudioSource;



    public void StartButton()
    {
        Debug.Log("Start Button pressed - loading game");
        SceneManager.LoadScene("TutorialScene");
        buttonAudioSource.Play();
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
        optionsMenu.SetActive(true);
        
    }

    public void CloseOptionsMenu()
    {
        
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        buttonAudioSource.Play();
    }

    public void OpenCustomisationMenu()
    {
        buttonAudioSource.Play();
        mainMenu.SetActive(false);
        customisationMenu.SetActive(true);
        
    }

    public void CloseCustomisationMenu()
    {
        
        mainMenu.SetActive(true);
        customisationMenu.SetActive(false);
        buttonAudioSource.Play();
    }

}