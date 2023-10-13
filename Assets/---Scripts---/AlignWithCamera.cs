using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    void FixedUpdate()
    {
        Vector3 Direction = (transform.position - Camera.main.transform.position).normalized;
        Direction.y = 0;
        transform.rotation = Quaternion.LookRotation(Direction,Vector3.up);
    }
}
