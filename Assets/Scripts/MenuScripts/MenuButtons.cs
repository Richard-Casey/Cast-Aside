using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;


    public void StartButton()
    {
        Debug.Log("Start Button pressed - loading game");
        SceneManager.LoadScene("SampleScene");
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
}