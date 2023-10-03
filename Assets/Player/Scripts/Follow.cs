using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;

    public Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = Target.position + (Offset); 
        transform.position = Position;
    }
}
