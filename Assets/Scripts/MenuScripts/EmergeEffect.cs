using UnityEngine;

public class EmergeEffect : MonoBehaviour
{
    public float emergeTime = 1.5f;
    public float startYOffset = -100f; 
    private float timer = 0f;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        transform.localScale = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y + startYOffset, transform.position.z);
    }

    void Update()
    {
        if (timer < emergeTime)
        {
            timer += Time.deltaTime;
            float progress = timer / emergeTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, originalPosition.y + startYOffset, transform.position.z), originalPosition, progress);
        }
    }
}