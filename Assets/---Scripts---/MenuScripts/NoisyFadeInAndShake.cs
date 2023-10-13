using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoisyFadeInAndShake : MonoBehaviour
{
    public float fadeInTime = 2.0f;
    public float startDelay = 1.0f;
    public float shakeAmount = 13.5f;
    public float shakeDuration = 0.5f;
    public float waitTime = 2.5f;

    private Image image;
    private Color originalColor;
    private float timer = 0.0f;
    private bool fadeInComplete = false;
    private Vector3 preShakePosition;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(startDelay);

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        fadeInComplete = true;
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(ShakeRoutine());
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        preShakePosition = transform.position;

        while (elapsed < shakeDuration)
        {
            transform.position = preShakePosition + Random.insideUnitSphere * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = preShakePosition;
    }
}
