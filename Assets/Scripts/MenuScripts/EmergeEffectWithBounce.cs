using UnityEngine;
using System.Collections;

public class EmergeEffectWithBounce : MonoBehaviour
{
    public float emergeTime = 1.5f;
    public float bounceHeight = 10f; 
    public float bounceTime = 0.2f; 
    public float scaleOvershoot = 1.2f; 
    private float timer = 0f;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool hasBounced = false; 

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        transform.localScale = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
    }

    void Update()
    {
        if (timer < emergeTime)
        {
            timer += Time.deltaTime;
            float progress = timer / emergeTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, originalPosition.y - 10, transform.position.z), originalPosition, progress);
        }
        else if (!hasBounced)
        {
            StartCoroutine(Bounce());
            hasBounced = true;
        }
    }

    IEnumerator Bounce()
    {
        float bounceTimer = 0f;
        Vector3 bounceTarget = new Vector3(originalPosition.x, originalPosition.y + bounceHeight, originalPosition.z);
        Vector3 scaleTarget = originalScale * scaleOvershoot;

        while (bounceTimer < bounceTime)
        {
            bounceTimer += Time.deltaTime;
            float progress = bounceTimer / bounceTime;
            transform.position = Vector3.Lerp(originalPosition, bounceTarget, progress);
            transform.localScale = Vector3.Lerp(originalScale, scaleTarget, progress);
            yield return null;
        }

        bounceTimer = 0f;

        while (bounceTimer < bounceTime)
        {
            bounceTimer += Time.deltaTime;
            float progress = bounceTimer / bounceTime;
            transform.position = Vector3.Lerp(bounceTarget, originalPosition, progress);
            transform.localScale = Vector3.Lerp(scaleTarget, originalScale, progress);
            yield return null;
        }
    }
}
