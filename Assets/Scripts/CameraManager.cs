using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform StartingPosition;
    [SerializeField] float TransitionSpeed = 1f;
    [SerializeField] float TransitionHeight = 5f;
    [SerializeField] List<Transform> RoomTargets;

    Transform CurrentTarget;
    Camera camera;
    float CurrentTime = 0;

    public void Start()
    {
        camera = GetComponent<Camera>();
        
        CurrentTarget = StartingPosition;
        
        transform.position = CurrentTarget.position;
    }

    public void StartTransition(Transform Target)
    {
        StartCoroutine(TransitionTo(Target));
    }

    IEnumerator TransitionTo(Transform Target)
    {
        while (transform.position.y < CurrentTarget.position.y + TransitionHeight)
        {
            transform.position = Vector3.Lerp(CurrentTarget.position, CurrentTarget.position + new Vector3(0,TransitionHeight,0), CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        CurrentTime = 0;
        transform.position = Target.position + new Vector3(0, TransitionHeight, 0);
        StartCoroutine(TransitionFrom(Target));
    }

    IEnumerator TransitionFrom(Transform Target)
    {
        while (transform.position.y > Target.position.y)
        {
            transform.position = Vector3.Slerp(Target.position+ new Vector3(0, TransitionHeight, 0), Target.position , CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        CurrentTarget = Target;
        CurrentTime = 0;


    }
    
    public void OnGUI()
    {
        foreach (var target in RoomTargets)
        {
            if (GUILayout.Button(target.name)) StartCoroutine(TransitionTo(target)); ;
        }
    }

    public void SetOrthographicSize(float size)
    {
        camera.orthographicSize = size;
    }

}
