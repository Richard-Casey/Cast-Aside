using System.Collections;
using UnityEngine;

public class CoffinShake : MonoBehaviour
{
    public float shakeAmount = 13.5f;
    public float shakeDuration = 0.5f;
    public float waitTime = 2.5f;

    private Vector3 preShakePosition;

    void Start()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        preShakePosition = transform.position; // Save the position before shaking

        while (elapsed < shakeDuration)
        {
            transform.position = preShakePosition + Random.insideUnitSphere * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = preShakePosition; // Reset to the position before shaking
    }
}