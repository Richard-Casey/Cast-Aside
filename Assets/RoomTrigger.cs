using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] CameraManager _cameraManager;
    [SerializeField] Transform _roomCameraPivot;

    void OnTriggerEnter()
    {
        //_cameraManager.SetTargetPosition(_roomCameraPivot.position);
    }
}
