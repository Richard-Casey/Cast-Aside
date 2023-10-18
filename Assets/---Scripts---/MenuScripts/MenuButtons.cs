using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject customisationMenu;


    public void StartButton()
    {
        Debug.Log("Start Button pressed - loading game");
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitButton()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OpenCustomisationMenu()
    {
        mainMenu.SetActive(false);
        customisationMenu.SetActive(true);
    }

    public void CloseCustomisationMenu()
    {
        mainMenu.SetActive(true);
        customisationMenu.SetActive(false);
    }
}