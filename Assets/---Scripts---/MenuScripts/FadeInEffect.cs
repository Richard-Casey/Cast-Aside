using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public float fadeInTime = 2.0f; 
    public float delayTime = 2.5f; 
    private CanvasGroup canvasGroup;
    private float timer = 0f;
    private bool startFading = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // Start fully transparent
        Invoke("StartFading", delayTime); 
    }

    void StartFading()
    {
        startFading = true;
    }

    void Update()
    {
        if (startFading)
        {
            if (timer < fadeInTime)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeInTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress); // Fade in
            }
        }
    }
}
