using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{


    #region Events

    public static UnityEvent<GameObject> Interaction = new UnityEvent<GameObject>();

    #endregion


    //public Texture2D CrosshairTexture;

    public Vector2 MouseInputDelta       { private set; get; }
    public Vector2 MousePositionOnScreen { private set; get; }
    public Vector2 MoveInput             { private set; get; }
    public bool    ConfineMouseInput     { private set; get; }
    public bool    IsSprining            { private set; get; }
    public bool    IsJumping             { private set; get; }
    public bool    IsKneel               { private set; get; }
    public bool    IsCrouch              { private set; get; }
    public float   RotateInput           { private set; get; }
    public float   InOutInput            { private set; get; }
    public bool    isInteract             { private set; get; }
    public bool    isAttack1             { private set; get; }
    public bool    isAttack2             { private set; get; }
    #region Attack

    //void OnAttack1(InputValue value) => SetAttack1(value.isPressed);

    private void SetAttack1(bool value)
    {
        isAttack1 = value;
    }

    //void OnAttack2(InputValue value) => SetAttack2(value.isPressed);

    private void SetAttack2(bool value)
    {
        isAttack2 = value;
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        isAttack1 = value.performed;

    }


    public void OnAttack2(InputAction.CallbackContext value)
    {
        SetAttack2(value.performed);
    }


    public void OnInteract(InputAction.CallbackContext value) => SetInteract(value.started);

    private void SetInteract(bool value)
    {
        isInteract = value;
        if(value)Interaction?.Invoke(this.gameObject);
    }


    public void Update()
    {
        //Debug.Log(isInteract);
    }

    #endregion

    #region Focus
    private void OnApplicationFocus(bool hasFocus) => SetCursorState(false);
    private void OnApplicationPause() => SetCursorState(true);

    private void SetCursorState(bool newState)
    {
        ConfineMouseInput = newState;
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Confined;
        //Cursor.SetCursor(CrosshairTexture, new Vector2(CrosshairTexture.width/2f, CrosshairTexture.height/2f),CursorMode.Auto);
    }

    #endregion

    #region Move
    public void OnMove(InputAction.CallbackContext value) => SetNewMoveInput(value.ReadValue<Vector2>());
    private void SetNewMoveInput(Vector2 newMoveInput)
    {
        MoveInput = newMoveInput;
    }

    public void OnSprint(InputAction.CallbackContext value) => SetNewSprintInput(value.performed);

    private void SetNewSprintInput(bool newSprintInput)
    {
        IsSprining = newSprintInput;
    }

    public void OnJump(InputAction.CallbackContext value) => SetNewJumpInput(value.performed);

    private void SetNewJumpInput(bool newJumpInput)
    {
        IsJumping = newJumpInput;
    }

    public void OnKneel(InputAction.CallbackContext value) => SetNewKneelInput(value.performed);

    private void SetNewKneelInput(bool newKneelInput)
    {
        if (newKneelInput) IsKneel = !IsKneel;
    }

    public void OnCrouch(InputAction.CallbackContext value) => SetNewCrouchInput(value.performed);

    private void SetNewCrouchInput(bool newCrouchInput)
    {
        if (newCrouchInput) IsCrouch = !IsCrouch;
    }
    #endregion

    #region Look
    public void OnLook(InputAction.CallbackContext value) => SetNewMouseDelta(value.ReadValue<Vector2>());
    private void SetNewMouseDelta(Vector2 newDelta)
    {
        MouseInputDelta = newDelta;
        MousePositionOnScreen = Mouse.current.position.value;
    }

    #region Rotate

    public void OnRotate(InputAction.CallbackContext value) => SetNewLockState(value.ReadValue<float>());
    private void SetNewLockState(float newInput)
    {
        RotateInput = newInput;
    }

    #endregion

    #region InOut

    public void OnInOut(InputAction.CallbackContext value) => SetNewInOut(value.ReadValue<float>());
    private void SetNewInOut(float newIo)
    {
        InOutInput = newIo;
    }

    #endregion
    #endregion
}
