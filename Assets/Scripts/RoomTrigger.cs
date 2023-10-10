using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] Roomtypes RoomType;

    public enum Roomtypes
    {
        Puzzle,
        Replenish
    }

    public static UnityEvent<Vector3,Roomtypes> RoomEntered = new UnityEvent<Vector3, Roomtypes>();

    void OnTriggerEnter()
    {
        RoomEntered?.Invoke(transform.position, RoomType);
    }
}
