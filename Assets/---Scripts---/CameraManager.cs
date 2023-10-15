using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    //Events
    public static UnityEvent TransitionToCompleted = new UnityEvent();
    public static UnityEvent TransitionCompleted = new UnityEvent();

    //Refrences
    [SerializeField] Transform PlayerTransform;
    [SerializeField] RawImage FadeToBlackImage;
    [SerializeField] Vector3 _cameraOffset = new Vector3(15,15,15);
    [SerializeField] Ease _easeType = Ease.InOutCirc;
    [SerializeField] float _transitionTime = 4f;

    //Local Variables
    Vector3 _currentCameraOffset;
    Vector3 _cameraDefultRotation;
    Vector3 _currentPositon = Vector3.zero;
    bool _shouldAutoUpdate = true;
    float _t = 0;


    public void Start()
    {
        _cameraDefultRotation = transform.eulerAngles;
        _currentCameraOffset = _cameraOffset;

        transform.position = PlayerTransform.position + _cameraOffset;
        _currentPositon = transform.position;
    }
    

    private float Round(float Input, int RoundTo)
    {
        return (Input / RoundTo) * RoundTo;
    }

    public void Update()
    {
        if (_shouldAutoUpdate)
        {
            Vector3 TargetPosition = PlayerTransform.position;

            //Add The Cameras Offset To The Center Of The room
            TargetPosition += _currentCameraOffset;

            //If The camera isnt already at the position move it over x amount of seconds
            if (TargetPosition != _currentPositon)
            {
                _t += Time.deltaTime;
                transform.position = Vector3.Lerp(_currentPositon, TargetPosition,_t);
            }
            else
            {
                _t = 0;
            }

            //And ones its approached stop the move
            if (transform.position.Equals(TargetPosition))
            {
                _currentPositon = TargetPosition;
            }
        }
    }


    public void SetCameraRotationX(float Rotation)
    {
        transform.DORotate(new Vector3(Rotation, transform.eulerAngles.y, transform.eulerAngles.z), _transitionTime).SetEase(_easeType); ;
    }
    public void SetCameraRotationY(float Rotation)
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x, Rotation, transform.eulerAngles.z), _transitionTime).SetEase(_easeType); ;
    }
    public void SetCameraRotationZ(float Rotation)
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Rotation), _transitionTime).SetEase(_easeType); ;
    }

    public void SetCameraOffsetX(float x)
    {
        _currentCameraOffset = new Vector3(x,_cameraOffset.y, _cameraOffset.z);
    }

    public void SetCameraOffsetz(float z)
    {
        _currentCameraOffset = new Vector3(_cameraOffset.x, _cameraOffset.y, z);
    }

    public void ResetTransform()
    {
        _currentCameraOffset = _cameraOffset;
        transform.DORotate(_cameraDefultRotation, _transitionTime).SetEase(_easeType);
    }
}
