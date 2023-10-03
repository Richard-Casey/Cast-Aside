#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

public class SunMover : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float radius = 10f;

    private Transform sun; 

    void Start()
    {
        sun = transform.GetChild(0); 
        UpdateSunPosition();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        UpdateSunPosition();
    }

    void UpdateSunPosition()
    {
        sun.localPosition = new Vector3(Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad) * radius);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, radius);
    }
#endif
}