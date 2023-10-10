using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TeleporterHandler : MonoBehaviour
{
    public static UnityEvent<Transform,CameraManager.TransitionType> Teleported = new UnityEvent<Transform, CameraManager.TransitionType>();

    [SerializeField] Transform TargetPoint;

    [SerializeField] public CameraManager.TransitionType transitionType;

    Transform LastCollidedTransform;

    public void OnTriggerEnter(Collider collision)
    {
        LastCollidedTransform = collision.transform;
        Teleported?.Invoke(TargetPoint, transitionType);
    }

    public void Start()
    {
        CameraManager.TransitionToCompleted.AddListener(Teleport);
    }

    public void Destory()
    {
        CameraManager.TransitionToCompleted.RemoveListener(Teleport);
    }

    void Teleport()
    {
        if (LastCollidedTransform != null)
        {
            LastCollidedTransform.position = TargetPoint.position;
        }
        LastCollidedTransform = null;
    }
}
