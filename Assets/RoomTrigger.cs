using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public static UnityEvent<Vector3> RoomEntered = new UnityEvent<Vector3>();

    void OnTriggerEnter()
    {
        RoomEntered?.Invoke(transform.position);
    }
}
