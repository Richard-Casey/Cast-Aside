using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LerpFollow : MonoBehaviour
{
    [SerializeField] Transform Target;
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position,Time.fixedDeltaTime);
    }
}
