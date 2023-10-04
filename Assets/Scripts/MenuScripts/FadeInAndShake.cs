using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInAndShake : MonoBehaviour
{
    public float fadeInTime = 2.0f;
    public float startDelay = 4.0f;
    public float shakeAmount = 13.5f;
    public float shakeDuration = 0.5f;
    public float cycleTime = 20.0f;
    public int minShakesPerCycle = 1;
    public int maxShakesPerCycle = 3;

    private Image image;
    private Color originalColor;
    private float timer = 0f;
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

        StartCoroutine(ShakeCycle());
    }

    IEnumerator ShakeCycle()
    {
        while (true)
        {
            int shakesThisCycle = Random.Range(minShakesPerCycle, maxShakesPerCycle + 1);
            for (int i = 0; i < shakesThisCycle; i++)
            {
                float randomTime = Random.Range(0, cycleTime / shakesThisCycle);
                yield return new WaitForSeconds(randomTime);
                StartCoroutine(Shake());
            }
            yield return new WaitForSeconds(cycleTime);
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
