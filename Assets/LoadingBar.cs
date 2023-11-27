using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] Slider uiSlider;
    [SerializeField] float ExtraLoadTime = 1f;


    void Start()
    {
        StartCoroutine(LoadSceneAsync("TutorialScene"));


    }

    float t = 0;
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            uiSlider.value = operation.progress;
            yield return null;
        }

    }


}
