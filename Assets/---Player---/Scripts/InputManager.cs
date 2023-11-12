using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{


    #region Events

    public static UnityEvent<GameObject> Interaction = new UnityEvent<GameObject>();
    public static UnityEvent OnAttackRelease = new UnityEvent();
    public static UnityEvent onPausePress = new UnityEvent();

    #endregion


    //public Texture2D CrosshairTexture;

    public static Vector2 MouseInputDelta       { private set; get; }
    public static Vector2 MousePositionOnScreen { private set; get; }
    public static Vector2 MoveInput             { private set; get; }
    public static bool ConfineMouseInput     { private set; get; }
    public static bool IsSprining            { private set; get; }
    public static bool IsJumping             { private set; get; }
    public static bool IsKneel               { private set; get; }
    public static bool IsCrouch              { private set; get; }
    public static float RotateInput           { private set; get; }
    public static bool CameraRotateLeftInput { private set; get; }
    public static bool CameraRotateRightInput { private set; get; }
    public static float InOutInput            { private set; get; }
    public static bool isInteract            { private set; get; }
    public static InputAction.CallbackContext isAttack1             { private set; get; }
    public static bool isAttack2             { private set; get; }
    public static bool onPause { private set; get; }
    #region Attack




    private void SetAttack2(bool value)
    {
        isAttack2 = value;
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        if(value.canceled) OnAttackRelease?.Invoke();
        isAttack1 = value;
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
    public void OnPause(InputAction.CallbackContext value) => SetPause(value.started);

    private void SetPause(bool value)
    {
        onPause = value;
        if (value) onPausePress?.Invoke();
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

    public void OnRotate(InputAction.CallbackContext value) => SetNewRotate(value.ReadValue<float>());
    private void SetNewRotate(float newInput)
    {
        RotateInput = newInput;
    }

    public void OnCameraRotateLeft(InputAction.CallbackContext value) => SetNewCameraRotateLeft(value.started);
    private void SetNewCameraRotateLeft(bool newInput)
    {
        //add events
        CameraRotateRightInput = newInput;

    }

    public void OnCameraRotateRight(InputAction.CallbackContext value) => SetNewCameraRotateRight(value.started);
    private void SetNewCameraRotateRight(bool newInput)
    {
        CameraRotateRightInput = newInput;
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
