using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampedMove : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    public void MoveUp(float MaxHeight)
    {
        Vector3 CurrentHeight = transform.localPosition;
        CurrentHeight.y += speed * Time.deltaTime;
        CurrentHeight.y = Mathf.Clamp(CurrentHeight.y, CurrentHeight.y, MaxHeight);
        transform.localPosition = CurrentHeight;
    }

    public void MoveDown(float MinHeight)
    {
        Vector3 CurrentHeight = transform.localPosition;
        CurrentHeight.y -= speed * Time.deltaTime;
        CurrentHeight.y = Mathf.Clamp(CurrentHeight.y, MinHeight, CurrentHeight.y);
        transform.localPosition = CurrentHeight;
    }
}
