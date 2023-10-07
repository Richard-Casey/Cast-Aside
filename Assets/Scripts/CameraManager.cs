using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
public class CameraManager : MonoBehaviour
{
    public static UnityEvent TransitionToCompleted = new UnityEvent();

    [SerializeField] Transform StartingPosition;
    [SerializeField] float TransitionSpeed = 1f;
    [SerializeField] float TransitionHeight = 5f;
    [SerializeField] List<Transform> RoomTargets;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] float FollowSpeed = 8f;

    bool ShouldAutoUpdate = true;
    Vector3 _currentPositon = Vector3.zero;
    float t = 0;

    Transform CurrentTarget;
    Camera camera;
    float CurrentTime = 0;

    public void Start()
    {
        TeleporterHandler.Teleported.AddListener(StartTransition);

        camera = GetComponent<Camera>();

        CurrentTarget = StartingPosition;

        transform.position = CurrentTarget.position;
        _currentPositon = transform.position;
    }
 
    public void OnDestroy()
    {
        TeleporterHandler.Teleported.RemoveListener(StartTransition);
    }


    private float Round(float Input, int RoundTo)
    {
        return (Input / RoundTo) * RoundTo;
    }

    public void Update()
    {
        if (ShouldAutoUpdate)
        {
            //Take the players position and round it To The Nearest Multiple of 30
            Vector3 TargetPosition = PlayerTransform.position;
            
            /*TargetPosition /= 30;

            TargetPosition = new Vector3(Mathf.Round(TargetPosition.x), Mathf.Round(TargetPosition.y),
                Mathf.Round(TargetPosition.z));

            TargetPosition *= 30;*/
            
            //Add The Cameras Offset To The Center Of The room
            TargetPosition += new Vector3(15, 15, 15);

            //If The camera isnt already at the position move it over x amount of seconds
            if (TargetPosition != _currentPositon)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(_currentPositon, TargetPosition,t);
            }
            else
            {
                t = 0;
            }

            //And ones its approached stop the move
            if (transform.position.Equals(TargetPosition))
            {
                _currentPositon = TargetPosition;
            }
        }
    }

    //Public wrapper
    public void StartTransition(Transform Target)
    {
        ShouldAutoUpdate = false;
        StartCoroutine(TransitionTo(Target));
    }


    //Handles the transition to a point
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
        TransitionToCompleted?.Invoke();
        StartCoroutine(TransitionFrom(Target));
    }

    //Handles a transition from a point
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

        ShouldAutoUpdate = true;
    }
    
    //Dras a list of debug locations for the camera
    public void OnGUI()
    {
        foreach (var target in RoomTargets)
        {
            if (GUILayout.Button(target.name)) StartCoroutine(TransitionTo(target)); ;
        }
    }

    /*public void SetOrthographicSize(float size)
    {
        camera.orthographicSize = size;
    }*/

}
