using UnityEngine;

public class MoveAndScaleLogo : MonoBehaviour
{
    public float startYDelay = 4.0f; // Time to start the action
    public float moveTime = 1.0f; // Time to complete the action
    public float targetScale = 0.5f; // The scale to reduce to
    public float moveYUnits = 10.0f; // Units to move up the screen

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private float timer = 0f;
    private bool actionStarted = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        Invoke("StartAction", startYDelay); // Start the action after startYDelay seconds
    }

    void StartAction()
    {
        actionStarted = true;
    }

    void Update()
    {
        if (actionStarted)
        {
            if (timer < moveTime)
            {
                timer += Time.deltaTime;
                float progress = timer / moveTime;

                // Scale the object
                transform.localScale = Vector3.Lerp(originalScale, new Vector3(targetScale, targetScale, originalScale.z), progress);

                // Move the object
                transform.position = Vector3.Lerp(originalPosition, new Vector3(originalPosition.x, originalPosition.y + moveYUnits, originalPosition.z), progress);
            }
        }
    }
}