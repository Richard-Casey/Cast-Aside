using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPushableBlock : MonoBehaviour
{

    [SerializeField] float MovementStrength = 5f;

    Vector3 DefaultLocation;
    Quaternion DefaultRotation;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefaultLocation = transform.position;
        DefaultRotation = transform.rotation;
    }

    public void Push()
    {
        rb.AddForce(MovementStrength * MathUtil.Flatten(GameSpecificCharacterController.CurrentSunForward,Vector3.up) + (Vector3.down * 40f),ForceMode.Impulse);

    }

    public void ResetBlock()
    {
        transform.position = DefaultLocation;
        transform.rotation = DefaultRotation;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            ResetBlock();
        }
    }
}
