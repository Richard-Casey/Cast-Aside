using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using DG.Tweening;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] CharacterController controller;
    //Local Variables
    Vector3 _currentCameraOffset;
    Vector3 _cameraDefultRotation;
    Vector3 _currentPositon = Vector3.zero;
    bool _shouldAutoUpdate = true;
    float _t = 0;

    public Vector3 GetOffset => _cameraOffset;
    public void Start()
    {
        _cameraDefultRotation = transform.eulerAngles;
        _currentCameraOffset = _cameraOffset;

        transform.position = PlayerTransform.position + _cameraOffset;
        _currentPositon = transform.position;
    }


    void OnEnable()
    {
        InputManager.OnAttackRelease.AddListener(SnapRotation);
    }

    void OnDisable()
    {
        InputManager.OnAttackRelease.RemoveListener(SnapRotation);
    }

    private float Round(float Input, float RoundTo)
    {
        return Mathf.Round(Input / RoundTo) * RoundTo;
    }


    //Handle CameraRotationAndSnapping

    bool isBusy = false;
    bool isRotating = false;
    [SerializeField]float RotateSpeed = 2f;
    [SerializeField] float RotationSnapAngle = 45;
    void HandleRotation()
    {
        isRotating = (InputManager.isAttack1.started || InputManager.isAttack1.performed);
        if (isRotating && !isBusy)
        {
            transform.Rotate(new Vector3(0,InputManager.MouseInputDelta.x * RotateSpeed , 0),Space.World);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    void SnapRotation()
    {
        isBusy = true;
        Vector3 SnappedVector = new Vector3(transform.eulerAngles.x, Round(transform.eulerAngles.y, RotationSnapAngle),transform.eulerAngles.z);
        transform.DORotate(SnappedVector, .5f).SetEase(Ease.OutBounce).OnComplete(() => isBusy = false);
    }


    public void Update()
    {
        HandleRotation();

        //Handle moving the cameras position
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


    public void SetCameraPosition(Transform target)
    {
        _shouldAutoUpdate = false;
        transform.DOMove(target.position,_transitionTime,false);
        transform.DORotate(target.eulerAngles, _transitionTime);
    }

    #region RotationFunctions

    


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


    public void SetCameraRotation(float x, float y, float z)
    {
        transform.DORotate(new Vector3(x, y, z), _transitionTime).SetEase(_easeType);

    }

    public void SetCameraRotationX(float Rotation,float time, Ease ease)
    {
        transform.DORotate(new Vector3(Rotation, transform.eulerAngles.y, transform.eulerAngles.z), time).SetEase(ease); ;
    }
    public void SetCameraRotationY(float Rotation, float time, Ease ease)
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x, Rotation, transform.eulerAngles.z), time).SetEase(ease); ;
    }
    public void SetCameraRotationZ(float Rotation, float time, Ease ease)
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Rotation), time).SetEase(ease); ;
    }

    public void SetCameraOffsetX(float x)
    {
        _currentCameraOffset = new Vector3(x,_cameraOffset.y, _cameraOffset.z);
    }

    public void SetCameraOffsetz(float z)
    {
        _currentCameraOffset = new Vector3(_cameraOffset.x, _cameraOffset.y, z);
    }

    public Vector3 GetCameraEuler()
    {
        return transform.eulerAngles;
    }

    public void ResetTransform()
    {
        _shouldAutoUpdate = true;
        _currentCameraOffset = _cameraOffset;
        transform.DORotate(_cameraDefultRotation, _transitionTime).SetEase(_easeType);
    }
    #endregion
}
