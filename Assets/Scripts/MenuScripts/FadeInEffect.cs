using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public float fadeInTime = 2.0f; // Time it takes to fade in, in seconds
    public float delayTime = 2.5f; // Time to wait before starting the fade in, in seconds
    private CanvasGroup canvasGroup;
    private float timer = 0f;
    private bool startFading = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // Start fully transparent
        Invoke("StartFading", delayTime); // Invoke the StartFading function after delayTime seconds
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
