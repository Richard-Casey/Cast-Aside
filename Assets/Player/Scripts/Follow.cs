using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;

    public Vector3 Offset;

    public CinemachineTargetGroup group;

    // Update is called once per frame
    void Update()
    {

        Vector3 Position = Target.position + (Offset);
        transform.position = Position;
    }

    public void LockedPosition(Transform newPosition)
    {

        group.AddMember(newPosition,2,4);
    }

    public void UnLockPosition(Transform newPosition)
    {
        group.RemoveMember(newPosition);
    }
}
