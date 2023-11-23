using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CompletionPoint : MonoBehaviour
{
    public void OnComplete()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
