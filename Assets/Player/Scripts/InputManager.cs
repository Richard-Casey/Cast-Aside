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

    void OnAttack1(InputValue value)
    {
        isAttack1 = value.isPressed;

    }


    void OnAttack2(InputValue value)
    {
        SetAttack2(value.isPressed);

    }


    void OnInteract(InputValue value) => SetInteract(value.isPressed);

    private void SetInteract(bool value)
    {
        isInteract = value;
        if(value)Interaction?.Invoke(this.gameObject);
    }


    void Update()
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
    void OnMove(InputValue value) => SetNewMoveInput(value.Get<Vector2>());
    private void SetNewMoveInput(Vector2 newMoveInput)
    {
        MoveInput = newMoveInput;
    }

    void OnSprint(InputValue value) => SetNewSprintInput(value.isPressed);

    private void SetNewSprintInput(bool newSprintInput)
    {
        IsSprining = newSprintInput;
    }

    void OnJump(InputValue value) => SetNewJumpInput(value.isPressed);

    private void SetNewJumpInput(bool newJumpInput)
    {
        IsJumping = newJumpInput;
    }

    void OnKneel(InputValue value) => SetNewKneelInput(value.isPressed);

    private void SetNewKneelInput(bool newKneelInput)
    {
        if (newKneelInput) IsKneel = !IsKneel;
    }

    void OnCrouch(InputValue value) => SetNewCrouchInput(value.isPressed);

    private void SetNewCrouchInput(bool newCrouchInput)
    {
        if (newCrouchInput) IsCrouch = !IsCrouch;
    }
    #endregion

    #region Look
    void OnLook(InputValue value) => SetNewMouseDelta(value.Get<Vector2>());
    private void SetNewMouseDelta(Vector2 newDelta)
    {
        MouseInputDelta = newDelta;
        MousePositionOnScreen = Mouse.current.position.value;
    }

    #region Rotate

    void OnRotate(InputValue value) => SetNewLockState(value.Get<float>());
    private void SetNewLockState(float newInput)
    {
        RotateInput = newInput;
    }

    #endregion

    #region InOut

    void OnInOut(InputValue value) => SetNewInOut(value.Get<float>());
    private void SetNewInOut(float newIo)
    {
        InOutInput = newIo;
    }

    #endregion
    #endregion
}
