using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] bool MatchX = false;
    [SerializeField] bool MatchY = false;
    [SerializeField] bool MatchZ = false;

    void Update()
    {
        Vector3 TargetRotation = Target.eulerAngles;

        TargetRotation.x = MatchX ? TargetRotation.x : transform.eulerAngles.x;
        TargetRotation.y = MatchY ? TargetRotation.y : transform.eulerAngles.y;
        TargetRotation.z = MatchZ ? TargetRotation.z : transform.eulerAngles.z;

        transform.eulerAngles = TargetRotation;
    }
}
