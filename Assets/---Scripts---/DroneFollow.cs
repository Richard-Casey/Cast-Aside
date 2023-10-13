using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float OffsetDistance = 3f;
    [SerializeField] float MoveSpeed = 3f;
    [SerializeField] float RotationSpeed = 1f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out rb))
        {
            rb = transform.AddComponent<Rigidbody>();

        }
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relative = transform.position - (target.position + Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(relative);
        Quaternion current = transform.rotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * RotationSpeed);
        float Distance = Vector3.Distance(transform.position,
            new Vector3(target.position.x, transform.position.y, target.position.z));
        if (Distance > OffsetDistance)
        {
            rb.AddForce(-transform.forward, ForceMode.Force);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 2f);
        }



    }
}
