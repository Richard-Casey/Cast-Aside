using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    #region Refrences
    [Header("Refrences")]

    //Public
    public InputManager Input;
    [SerializeField] Transform Camera;

    //Private
    Animator Animator;
    Rigidbody rb;
    CapsuleCollider collider;
    #endregion

    #region Booleans
    public bool HasAnimator { private set; get;}
    public bool HasRigidbody { private set; get; }
    public bool isGrounded { private set; get; }
    public bool isSloped;
    public bool isIdle { private set; get; }
    public bool isMoveable { private set; get; }
    public bool _jumped { private set; get; }

    [SerializeField] bool CanCrouch = true;
    [SerializeField] bool CanKneel = false;
    [SerializeField] bool IgnoreXZRotations = true;
    [SerializeField] bool UseUpAxis = false;

    #endregion

    #region LayerMasks

    [Header("Layer Masks")] 
    public LayerMask GroundLayer;
    #endregion

    #region AnimatorIDs

    int SpeedID;
    int MotionSpeedID;
    int JumpID;
    int JumpCooldownID;
    int GroundedID;
    int KneelID;
    int CrouchedID;
    void GetAnimatorIDs()
    {
        SpeedID = Animator.StringToHash("Speed");
        MotionSpeedID = Animator.StringToHash("MotionSpeed");
        JumpID = Animator.StringToHash("Jump");
        JumpCooldownID = Animator.StringToHash("JumpCooldown");
        GroundedID = Animator.StringToHash("isGrounded");
        KneelID = Animator.StringToHash("isKneel");
        CrouchedID = Animator.StringToHash("isCrouched");
    }

    #endregion

    void Start()
    {
        HasAnimator = TryGetComponent<Animator>(out Animator);
        HasRigidbody = TryGetComponent<Rigidbody>(out rb);
        DefaultLookForward = CameraPivot.forward;
        TryGetComponent<CapsuleCollider>(out collider);
        //_cameraFollow = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        if (HasAnimator)GetAnimatorIDs();
    }

    void Update()
    {
        
        Jump();
        isGrounded = GroundCheck();
    }

    void FixedUpdate()
    {
        ControlDrag();
        Move();
    }

    public void SetIgnoreRotation(bool value)
    {
        IgnoreXZRotations = value;
    }

    #region Movement

    [Header("Movement",order = 0)] 
    [Header("Ground",order = 1)]
    public float IdleThreshold = .1f;
    public float WalkSpeed = 3f;
    public float RunSpeed = 6f;
    public float CrouchWalkMultiplier = .7f;
    public float CrouchRunMultiplier = .5f;
    public float MovementMultiplier = 25f;
    public float AccelerationTime = 1f;
    public float NewTargetMoveSpeed = 0f;
    public float TargetMoveSpeed = 3f;
    public float DecelerationSpeed = 12f;
    public float Gravity = -9.8f;
    public float Height = 2f;
    public float CrouchedHeight = 1.1f;
    public bool LockMovement = false;
    Vector3 DefaultLookForward = -Vector3.one;
    [SerializeField] Vector3 CurrentGroundNormal = Vector3.zero;

    [Header("Animations",order = 1)]
    public float RotationSmoothTime = .07f;

    //ANimation Time Scale
    float CurrentAnimationTimeScale = 0f;
    float NewAnimationTimeScale = 0f;
    public float StandingAnimationTimeScale = 6.5f;
    public float CrouchedAnimationTimeScale = 1f;
    float _animationBlend = 0f;

    Vector3 _inputDirection = Vector2.zero;
    Vector2 _velocity = Vector2.zero;
    
    float _rotationVelocity;


    public void SetUseUpAxis()
    {
        UseUpAxis = true;
    }

    public void ResetCustomAxis()
    {
        UseUpAxis = false;
    }

    void Move()
    {
        isIdle = InputManager.MoveInput.sqrMagnitude < IdleThreshold;

        isMoveable = (Animator.GetCurrentAnimatorStateInfo(0).IsTag("Moveable")&& !LockMovement);

        _inputDirection = InputManager.MoveInput.x * CameraPivot.right +
                          InputManager.MoveInput.y * new Vector3(CameraPivot.forward.x, 0, CameraPivot.forward.z);

        if (UseUpAxis)
        {
            _inputDirection = InputManager.MoveInput.x * CameraPivot.right +
                InputManager.MoveInput.y * Camera.up;

        }
        if (IgnoreXZRotations)
        {
            _inputDirection = InputManager.MoveInput.x * CameraPivot.right +
                              InputManager.MoveInput.y * new Vector3(DefaultLookForward.x, 0, DefaultLookForward.z);
        }

        _inputDirection = _inputDirection.normalized;

        _velocity = Vector2.right * rb.velocity.x + Vector2.up * rb.velocity.z;

        NewTargetMoveSpeed = InputManager.IsSprining ? RunSpeed : WalkSpeed;

        TargetMoveSpeed = Mathf.Lerp(TargetMoveSpeed, NewTargetMoveSpeed, Time.deltaTime * AccelerationTime);

        float MultipliedTargetMoveSpeed = TargetMoveSpeed * MovementMultiplier;

        if (InputManager.IsCrouch && CanCrouch)
            MultipliedTargetMoveSpeed = InputManager.IsSprining ? MultipliedTargetMoveSpeed * CrouchRunMultiplier : MultipliedTargetMoveSpeed * CrouchWalkMultiplier;


        NewAnimationTimeScale = InputManager.IsCrouch ? CrouchedAnimationTimeScale : StandingAnimationTimeScale;
        CurrentAnimationTimeScale = Mathf.Lerp(CurrentAnimationTimeScale, NewAnimationTimeScale, Time.deltaTime);
        _animationBlend = Mathf.Lerp(_animationBlend, _velocity.normalized.magnitude * NewAnimationTimeScale, Time.fixedDeltaTime * CurrentAnimationTimeScale);
        if(_animationBlend < .01f) _animationBlend = 0f;


        

        //Normal Grounded Input
        if (isGrounded && !_jumped && !(InputManager.IsKneel && CanKneel) )
        {
            //Handle Deceleration
            if (_inputDirection.magnitude == 0f && _velocity.magnitude >= IdleThreshold)
            {
                //Add Force In Oppisite Direction
                rb.AddForce(-new Vector3(rb.velocity.x, isGrounded ? (rb.velocity.y - Physics.gravity.y) : 0, rb.velocity.z) * (DecelerationSpeed * Time.fixedDeltaTime), ForceMode.Force);
            }else if (isGrounded && !isSloped)
            {//Handle Regular Movement
                Vector3 TargetVelocity =
                    Vector3.ClampMagnitude((_inputDirection * MultipliedTargetMoveSpeed * Time.fixedDeltaTime) + Vector3.down, MultipliedTargetMoveSpeed / 50);
                 Vector3 newVelocity = Vector3.MoveTowards(rb.velocity, TargetVelocity, TargetMoveSpeed);
                Vector3 Diffrence = newVelocity - rb.velocity;

                rb.AddForce(Diffrence, ForceMode.VelocityChange);
                

            }
            else if (isGrounded && isSloped)
            {//Handle Sloped Movement
                Vector3 TargetVelocity = Vector3.ClampMagnitude((Vector3.ProjectOnPlane(_inputDirection,CurrentGroundNormal) * MultipliedTargetMoveSpeed * Time.fixedDeltaTime) + Vector3.down, MultipliedTargetMoveSpeed / 50); 
                Vector3 newVelocity = Vector3.MoveTowards(rb.velocity, TargetVelocity, TargetMoveSpeed);
                Vector3 Diffrence = newVelocity - rb.velocity;
              
              rb.AddForce(Diffrence, ForceMode.VelocityChange);
              


            }
        }

        //If we are in the air handle air movement and apply gravity down
        else if (!isGrounded)
        {
            Vector3 TargetVelocity =
                Vector3.ClampMagnitude((_inputDirection * MultipliedTargetMoveSpeed * Time.fixedDeltaTime),
                    MultipliedTargetMoveSpeed / (50 / AirMovementMultiplier));

            Vector3 newVelocity = Vector3.MoveTowards(rb.velocity, TargetVelocity, 1);
            newVelocity.y = rb.velocity.y;
            Vector3 Diffrence = newVelocity-rb.velocity;
            rb.AddForce(Diffrence, ForceMode.VelocityChange);
            rb.AddForce(Vector3.up * Gravity , ForceMode.Acceleration);
        }

        //Rotate Player In Direction Of Movement
        if (InputManager.MoveInput != Vector2.zero)
        {
            Vector3 RotationInputDirection = new Vector3(_inputDirection.x, 0, _inputDirection.z).normalized;
            float _targetRotation = Mathf.Atan2(RotationInputDirection.x, RotationInputDirection.z) * Mathf.Rad2Deg + CameraPivot.forward.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        //Prevent Slight Velocities
        if (rb.velocity.magnitude < IdleThreshold)
        {
            rb.velocity = Vector3.zero;
        }

        //Handle Animations
        if (HasAnimator)
        {
            Animator.SetFloat(SpeedID,Mathf.Clamp(_animationBlend * rb.velocity.magnitude * 1.2f,0f,TargetMoveSpeed));
            Animator.SetFloat(MotionSpeedID,InputManager.MoveInput.magnitude);
            Animator.SetBool(KneelID,InputManager.IsKneel && CanKneel);
            Animator.SetBool(CrouchedID, InputManager.IsCrouch && CanCrouch);
        }

        collider.height = InputManager.IsCrouch ? CrouchedHeight : Height;
        collider.center = InputManager.IsCrouch ? new Vector3(0, CrouchedHeight / 2f, 0) : new Vector3(0, Height / 2f, 0);
    }
    [Header("Jumping", order = 1)]
    public float JumpCooldown = .7f;
    public float JumpMultiplier = 100f;
    public float AirMovementMultiplier = .75f;
    float CurrentJumpCooldown = 0f;
    void Jump()
    {
        if (isGrounded && InputManager.IsJumping && Animator.GetCurrentAnimatorStateInfo(0).IsTag("Moveable"))
        {
            //Add Force
            rb.AddForce(transform.up * JumpMultiplier, ForceMode.Impulse);
            //Set Cooldown
            CurrentJumpCooldown = JumpCooldown;
            //Trigger Animation
            if (HasAnimator) Animator.ResetTrigger(JumpID);
            if (HasAnimator) Animator.SetTrigger(JumpID);
        }
    }

    [Header("Drag Control")] 
    float AirDrag = 4f;
    float GroundDrag = 6f;

    private void ControlDrag()
    {
        rb.drag = isGrounded ? GroundDrag : AirDrag;
    }
    #endregion

    #region GroundCheck

    [Header("Ground Checking")] 
    public float Radius = .24f;
    public Vector3 Offset = new Vector3(0, -.12f,0);
    bool GroundCheck()
    {
        bool returnValue = Physics.CheckSphere(transform.position + Offset, Radius, GroundLayer);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitData, 15f, GroundLayer))
        {
            if (Vector3.Angle(Vector3.up, hitData.normal) > 20)
            {
                isSloped = true;
                CurrentGroundNormal = hitData.normal;
            }
            else
            {
                isSloped = false;
            }
        }
        else
        {
            isSloped = false;
        }
        if (HasAnimator) Animator.SetBool(GroundedID, returnValue);
        return returnValue;
    }

    #endregion

    #region Camera

    [Header("Camera")]
    public CinemachineVirtualCamera VirtualCamera;
    public Transform CameraPivot;
    public float RotationSensitivity = .45f;
    public float ZoomSensitivity = .1f;
    public float MinZoom = 2f;
    public float MaxZoom = 10f;
    float CameraOffsetMax = 5f;
    float CameraOffsetMin = 0f;
    Cinemachine3rdPersonFollow _cameraFollow;
    /*void Look()
    {
        CameraPivot.transform.eulerAngles = new Vector3(CameraPivot.transform.eulerAngles.x, CameraPivot.transform.eulerAngles.y + (InputManager.RotateInput * RotationSensitivity),
                CameraPivot.transform.eulerAngles.z);
        _cameraFollow.CameraDistance -= InputManager.InOutInput * ZoomSensitivity;
        _cameraFollow.CameraDistance = Mathf.Clamp(_cameraFollow.CameraDistance, MinZoom, MaxZoom);
        _cameraFollow.ShoulderOffset.z -= InputManager.InOutInput * ZoomSensitivity;
        _cameraFollow.ShoulderOffset.z = Mathf.Clamp(_cameraFollow.ShoulderOffset.z, CameraOffsetMin, CameraOffsetMax);


    }*/

    #endregion
    



}
