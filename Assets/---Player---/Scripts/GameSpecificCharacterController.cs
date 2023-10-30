using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameSpecificCharacterController : MonoBehaviour
{
    public static UnityEvent<float> DealDamage = new UnityEvent<float>();
    public static UnityEvent OnDeath = new UnityEvent();
    [SerializeField] CameraManager cameraManager;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator Animator;

    [SerializeField] InputManager Input;

    void Start()
    {
        TeleporterHandler.Teleported.AddListener(LockMovement);
        CameraManager.TransitionCompleted.AddListener(UnlockMovement);
        InputManager.CameraRotation.AddListener(RotateCamera);
        OnDeath.AddListener(OnPlayerDeath);
        DealDamage.AddListener(OnTakeDamage);
    }

    void OnDestroy()
    {
        TeleporterHandler.Teleported.RemoveListener(LockMovement);
        CameraManager.TransitionCompleted.RemoveListener(UnlockMovement);
        InputManager.CameraRotation.RemoveListener(RotateCamera);
        OnDeath.RemoveListener(OnPlayerDeath);
        DealDamage.RemoveListener(OnTakeDamage);
    }

    // Update is called once per frame
    void Update()
    {
        SunRotate();
        ListenForShadow();
        RechargeHealth();
    }

    void FixedUpdate()
    {
        Display();
    }


    #region LightController

    [Header("Sun Controller")]
    [SerializeField] Transform SunTransform;
    [SerializeField] float RotationSpeed = 1f;


    public static Vector3 CurrentSunForward;
    public float TimeSinceLastRotation;

    void SunRotate()
    {
        CurrentSunForward = SunTransform.forward;
        TimeSinceLastRotation += Time.deltaTime;

        //Check if the user is trying to rotate or that a rotate is possible
        if (CurrentHealth - HealthCostPerRotationFrame * Time.deltaTime < 0 || Input.RotateInput == 0) return;

        //Drain mana and roatate the sun by a fixed amount based on users input
        CurrentHealth -= HealthCostPerRotationFrame * Time.deltaTime;
        TimeSinceLastDamage = 0;
        float HorizontalRotation = Input.RotateInput;

        Vector3 SunsRotation = SunTransform.eulerAngles;
        SunsRotation.y += HorizontalRotation * Time.deltaTime * RotationSpeed;
        SunTransform.eulerAngles = SunsRotation;
    }

    [Header("Camera")]
    [SerializeField] float CameraRotationStep = 90f;

    void RotateCamera(int Direction)
    {
        Vector3 CamerasCurrentEuler = cameraManager.GetCameraEuler();
        cameraManager.SetCameraRotationY(CamerasCurrentEuler.y + Direction * CameraRotationStep, .75f, Ease.InOutSine);
    }

    #endregion

    #region ManaController

/*    [Header("Mana Controller")]
    [SerializeField] Slider ManaDisplay;
    [SerializeField] float CurrentMana = 0;
    [SerializeField] float MaxMana = 10;

    [SerializeField] float ManaRechargeRate = 1f;
    [SerializeField] float ManaRechargedPerCharge = 1f;
    [SerializeField] float RechargeDelay = 1f;

    public void RechargeMana()
    {
        //Handles recharging mana based on the time since last user rotation

        CurrentMana = Mathf.Clamp(CurrentMana + (ManaRechargedPerCharge * Time.deltaTime), 0, MaxMana);
    }*/


    void LockMovement(Transform transform)
    {
        controller.LockMovement = true;
    }

    void UnlockMovement()
    {
        controller.LockMovement = false;
    }

    #endregion

    #region HealthController

    [Header("Health Stats")]
    [SerializeField] Slider HealthDisplay;
    [SerializeField] float CurrentHealth;
    [SerializeField] float MaxHealth = 10;
    [SerializeField] float HealthRechargeRate = 1f;
    [SerializeField] float HealthRestoredPerTick = 1f;
    [SerializeField] float HealthRechargeDelay = 1f;
    [SerializeField] float HealthCostPerRotationFrame = 1;
    [SerializeField] float MaxDamage = 1f;
    [SerializeField] bool PauseManaDrain = true;
    float TimeSinceLastDamage;
    bool Dead;

    [Header("Respawn", order = 1)]
    [SerializeField] float DeathAnimationLength = 3.4f;
    [SerializeField] float HalfSceneTransitionLength = .5f;
    [SerializeField] Transform MostRecentSpawnPoint = null;
    [SerializeField] Vector3 FallBackSpawnPosition;
    public void RechargeHealth()
    {
        TimeSinceLastDamage += Time.deltaTime;

        //Dont regen straight after taking damage
        if (TimeSinceLastDamage < HealthRechargeDelay) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + HealthRestoredPerTick * Time.deltaTime, 0, MaxHealth);
    }


    [SerializeField] ParticleSystem HeadParticles;

    public void Display()
    {
        if (HealthDisplay)
        {
            HealthDisplay.value = CurrentHealth;
        }

        if (HeadParticles)
        {
            HeadParticles.emissionRate = CurrentHealth * 8f;
        }
    }

    public void OnTakeDamage(float Damage)
    {
        if(Dead)return;
        float DamageTaken = Mathf.Clamp(Damage, 0, MaxDamage);

        //Stop Negative and zero damage
        if (!(DamageTaken > 0)) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth - DamageTaken, 0, MaxHealth);

        TimeSinceLastDamage = 0f;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    [Header("Shadow-Tick-Damage")]
    [SerializeField] ShadowCube shadowDetection;
    [SerializeField] float HealthDrainPerSecond = .1f;

    public void ListenForShadow()
    {
        if (!shadowDetection.InShadow && !PauseManaDrain)
        {
            OnTakeDamage(HealthDrainPerSecond * Time.deltaTime);
            TimeSinceLastDamage = 0f;
        }
    }


    public void SetSpawnPoint(Transform spawnpoint)
    {
        MostRecentSpawnPoint = spawnpoint;
    }

    public void OnPlayerDeath()
    {
        Dead = true;
        //Play Dead Animation
        Animator.SetBool("IsDead",true);
        StartCoroutine(PlayTransitionAfterDeath());
    }

    IEnumerator PlayTransitionAfterDeath()
    {
        yield return new WaitForSeconds(DeathAnimationLength);
        SceneTransitions.PlayTransition?.Invoke(SceneTransitions.AnimationsTypes.CircleSwipe);
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(HalfSceneTransitionLength);
        if (MostRecentSpawnPoint) transform.position = MostRecentSpawnPoint.position;
        else transform.position = FallBackSpawnPosition;
        Animator.SetBool("IsDead", false);

    }

    #endregion
}
