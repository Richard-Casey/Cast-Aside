using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using DG.Tweening;
using Quaternion = UnityEngine.Quaternion;

public class Aim : MonoBehaviour
{
    public float m_XSensitivity = 1;
    public float m_YSensitivity = 1;
    public float NormalDistance = 2f;
    public float ScopedDistance = 1.5f;
    public float ScopeSpeed = 2f;
    public float AimRotationSpeed = 20f;
    public bool IsScoped = false;

    Vector2 CurrentLook;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform AimPoint;
    [SerializeField] CinemachineVirtualCamera vc;
    Cinemachine3rdPersonFollow tpf;


    [SerializeField] float IdleThreshold = .1f;
    [SerializeField] float IdleTurnThreshold = .1f;
    [SerializeField] float AngleThreshold = 40;

    public void Start()
    {
        tpf = vc.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    public void Update()
    {
        LookRotation();
        OnScope();
        RotatePlayer();
    }

    public void RotatePlayer()
    {
        if (Quaternion.Angle(PlayerTransform.rotation,transform.rotation) > AngleThreshold)
        {
            PlayerTransform.rotation = Quaternion.Lerp(PlayerTransform.rotation,transform.rotation, IdleTurnThreshold * Time.deltaTime);
        }
    }

    //Handle Aiming
    public void OnScope()
    {

        if (IsScoped)
        {
            Quaternion Target = Quaternion.RotateTowards(PlayerTransform.rotation, Quaternion.LookRotation(transform.forward, transform.up), Time.deltaTime * AimRotationSpeed);
            Target.x = 0;
            Target.z = 0;
            PlayerTransform.rotation = Target;
            
            
            AimPoint.rotation = Quaternion.RotateTowards(AimPoint.rotation, Quaternion.LookRotation(transform.forward, transform.up), Time.deltaTime * AimRotationSpeed);

        }
        else
        {
            AimPoint.rotation = Quaternion.RotateTowards(AimPoint.rotation, transform.rotation, Time.deltaTime * AimRotationSpeed);

        }
        if (InputManager.isAttack2 && !IsScoped)
        {
            IsScoped = true;
            DOTween.To(() => tpf.CameraDistance, x => tpf.CameraDistance = x, ScopedDistance, ScopeSpeed);
        }
        else if (!InputManager.isAttack2 && IsScoped)
        {
            IsScoped = false;
            DOTween.To(() => tpf.CameraDistance, x => tpf.CameraDistance = x, NormalDistance, ScopeSpeed);
        }
    }

    public void LookRotation()
    {
        if(InputManager.isPaused)return;

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        mouseInput.x *= m_XSensitivity;
        mouseInput.y *= m_YSensitivity;

        CurrentLook.x += mouseInput.x;
        CurrentLook.y = Mathf.Clamp(CurrentLook.y += mouseInput.y, -90, 90);

        transform.localRotation = Quaternion.Euler(CurrentLook.y, CurrentLook.x, 0);

    }
}
