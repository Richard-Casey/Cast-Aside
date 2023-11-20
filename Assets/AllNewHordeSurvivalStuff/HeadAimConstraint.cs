using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadAimConstraint : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] MultiAimConstraint AimConstraint;
    [SerializeField] Transform AimTarget;
    [SerializeField] float DotOffset = .5f;


    // Update is called once per frame
    void Update()
    {
        //Calculate the angle between the players forward and direction to aim target
        Vector3 PlayerForward = Player.transform.forward;
        Vector3 AimTargetDirection = (AimTarget.position - Player.transform.position).normalized;

        float dot = Vector3.Dot(PlayerForward, AimTargetDirection);
        dot += DotOffset;
        dot = Mathf.Clamp(dot,0, 1);

        AimConstraint.weight = dot;
        

    }
}
