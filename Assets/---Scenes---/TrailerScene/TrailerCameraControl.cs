using UnityEngine;

public class TrailerCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float dampingTime = 0.2f; // Time taken to smooth the camera movement

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Set target rotation
        targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Smoothly interpolate towards the target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, dampingTime * Time.deltaTime);
    }
}
