using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] Transform LookAtTarget;

    void Update()
    {
        Vector3 Directon = (LookAtTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Directon, Vector3.up);
    }
}
