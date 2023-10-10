using UnityEditor;
using UnityEngine;

public class SunMover : MonoBehaviour
{
    void Start()
    {
        RoomTrigger.RoomEntered.AddListener(Move);
    }

    void Destroy()
    {
        RoomTrigger.RoomEntered.RemoveListener(Move);
    }

    void Move(Vector3 position, RoomTrigger.Roomtypes type)
    {
        transform.position = position;
    }
}