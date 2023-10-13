using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPulse : MonoBehaviour
{
    public float pulseSpeed = 3.0f;
    public float pulseAmount = 0.01f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount + 1;
        transform.localScale = originalScale * scale;
    }
}
