using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static UnityEvent TransitionToCompleted = new UnityEvent();
    public static UnityEvent TransitionCompleted = new UnityEvent();

    //Refrences
    [SerializeField] Transform PlayerTransform;
    [SerializeField] RawImage FadeToBlackImage;

    [SerializeField] float TransitionSpeed = 1f;
    [SerializeField] float TransitionHeight = 5f;
    
    [SerializeField] float FollowSpeed = 8f;

    [SerializeField] bool ShouldFade = true;


    bool ShouldAutoUpdate = true;
    float t = 0;
    Vector3 _currentPositon = Vector3.zero;
    Vector3 CameraOffset = new Vector3(15,15,15);
    

    Camera camera;
    float CurrentTime = 0;

    public void Start()
    {
        TeleporterHandler.Teleported.AddListener(StartTransition);

        camera = GetComponent<Camera>();


        transform.position = PlayerTransform.position + CameraOffset;
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
            Vector3 TargetPosition = PlayerTransform.position;
            
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

    [System.Serializable]
    public enum TransitionType
    {
        Swipeup,
        Zoomout
    }

    //Public wrapper
    public void StartTransition(Transform Target,TransitionType type)
    {
        ShouldAutoUpdate = false;

        switch (type)
        {
            case TransitionType.Swipeup:
                //StartCoroutine();
                break;
            case TransitionType.Zoomout:
                if (ShouldFade) StartCoroutine(DoFade());
                StartCoroutine(ZoomOut(Target));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }


    float DefaultZoom = 15;
    [SerializeField] float TargetZoom = 100f;

    IEnumerator ZoomOut(Transform Target)
    {
        while (camera.orthographicSize < TargetZoom)
        {
            camera.orthographicSize = Mathf.Lerp(DefaultZoom, TargetZoom, CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        CurrentTime = 0;
        transform.position = Target.position + new Vector3(15,15,15);
        _currentPositon = transform.position;

        TransitionToCompleted?.Invoke();
        StartCoroutine(ZoomIn(Target));
    }

    IEnumerator ZoomIn(Transform Target)
    {
        while (camera.orthographicSize > DefaultZoom)
        {
            camera.orthographicSize = Mathf.Lerp(TargetZoom, DefaultZoom, CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        CurrentTime = 0;

        ShouldAutoUpdate = true;
        TransitionCompleted?.Invoke();
    }


    float FadeTime = 0f;
    IEnumerator DoFade()
    {
        while (FadeToBlackImage.color.a < 1)
        {
            Color CurrentColor = FadeToBlackImage.color;
            CurrentColor.a = Mathf.Lerp(0, 1, FadeTime);
            FadeToBlackImage.color = CurrentColor;
            FadeTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        FadeTime = 0;
        StartCoroutine(UndoFade());
    }

    IEnumerator UndoFade()
    {
        while (FadeToBlackImage.color.a > 0)
        {
            Color CurrentColor = FadeToBlackImage.color;
            CurrentColor.a = Mathf.Lerp(1, 0, FadeTime);
            FadeToBlackImage.color = CurrentColor;
            FadeTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        FadeTime = 0;
    }

    
    //Dras a list of debug locations for the camera
    public void OnGUI()
    {

    }

}
