using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpecificCharacterController : MonoBehaviour
{
    [SerializeField] InputManager Input;
    [SerializeField] CharacterController controller;
    void Start()
    {
        TeleporterHandler.Teleported.AddListener(LockMovement);
        CameraManager.TransitionCompleted.AddListener(UnlockMovement);
    }

    // Update is called once per frame
    void Update()
    {
        SunRotate();
        RechargeMana();
    }

    #region LightController

    [Header("Sun Controller")]
    [SerializeField] Transform SunTransform;
    [SerializeField] float RotationSpeed = 1f;

    [Header("Mana Controller")]
    [SerializeField] float CurrentMana = 0;
    [SerializeField] float MaxMana = 10;
    [SerializeField] float ManaCostPerSecond = 1;
    [SerializeField] float ManaRechargeRate = 1f;
    [SerializeField] float ManaRechargedPerCharge = 1f;
    [SerializeField] float RechargeDelay = 1f;

    public float TimeSinceLastRotation = 0f;

    void SunRotate()
    {
        TimeSinceLastRotation += Time.deltaTime;

        //Check if the user is trying to rotate or that a rotate is possible
        if (CurrentMana - (ManaCostPerSecond * Time.deltaTime ) < 0 || Input.RotateInput == 0) return;

        //Drain mana and roatate the sun by a fixed amount based on users input
        CurrentMana -= (ManaCostPerSecond * Time.deltaTime);
        TimeSinceLastRotation = 0;
        float HorizontalRotation = Input.RotateInput;

        Vector3 SunsRotation = SunTransform.eulerAngles;
        SunsRotation.y += HorizontalRotation * Time.deltaTime * RotationSpeed;
        SunTransform.eulerAngles = SunsRotation;
    }

    #endregion


    public void RechargeMana()
    {
        //Handles recharging mana based on the time since last user rotation
        if (TimeSinceLastRotation < RechargeDelay) return;
        CurrentMana = Mathf.Clamp(CurrentMana + (ManaRechargedPerCharge * Time.deltaTime), 0, MaxMana);
    }


    void LockMovement(Transform transform, CameraManager.TransitionType type)
    {
        controller.LockMovement = true;
    }

    void UnlockMovement()
    {
        controller.LockMovement = false;
    }
}
