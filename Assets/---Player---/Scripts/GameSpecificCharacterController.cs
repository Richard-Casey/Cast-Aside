using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameSpecificCharacterController : MonoBehaviour
{
    public static UnityEvent<float> DealDamage = new UnityEvent<float>();
    public static UnityEvent OnDeath = new UnityEvent();
    [SerializeField] CameraManager cameraManager;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator Animator;

    [SerializeField] InputManager Input;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] Texture2D CurrentCursorTexture;
    [SerializeField] Texture2D grabTexture;
    [SerializeField] Texture2D dragTexture;

    public void SetCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture,cursorTexture.Size() / 2f,CursorMode.Auto);
        CurrentCursorTexture = cursorTexture;
    }

    void Start()
    {
        SetCursor(grabTexture);
        renderer = transform.AddComponent<LineRenderer>();
        CameraManager.TransitionCompleted.AddListener(UnlockMovement);
        ObjectiveManager.ObjectiveComplete.AddListener(OnObjectivesComplete);
        OnDeath.AddListener(OnPlayerDeath);
        DealDamage.AddListener(OnTakeDamage);
    }

    public CameraManager GetCameraManager()
    {
        if (cameraManager) return cameraManager;
        return null;
    }

    [SerializeField] float TimeSinceLastObjective = 0;
    public void OnObjectivesComplete(Objective objective)
    {
        TimeSinceLastObjective = 0;
        PauseHintCounter = false;
    }       

    public CharacterController GetcCharacterController()
    {
        if (controller) return controller;
        return null;
    }
    void OnDestroy()
    {
        CameraManager.TransitionCompleted.RemoveListener(UnlockMovement);
        ObjectiveManager.ObjectiveComplete.RemoveListener(OnObjectivesComplete);
        OnDeath.RemoveListener(OnPlayerDeath);
        DealDamage.RemoveListener(OnTakeDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseHintCounter) TimeSinceLastObjective += Time.deltaTime;
        SunRotate();
        ListenForShadow();
        RechargeHealth();
    }

    void FixedUpdate()
    {
        Hint();
        Display();
    }




    [Header("Puzzle Hints")]
    [SerializeField] [Tooltip("Time In Seconds")] float HintTime = 120f;
    [SerializeField] GameObject PuzzleHint;
    [SerializeField] GameObject HintPointer;
    [SerializeField] PuzzleHintPointer HintPointerScript;
    [SerializeField] float HeightOffset = 3;
    [SerializeField] float DistanceOFfset = 3;
    [SerializeField] float DistanceBeforeRehint = 25f;
    [SerializeField] Objective ClosestObjective;

    bool PauseHintCounter = false;
    LineRenderer renderer;
    void Hint()
    {

        if(!ClosestObjective || !ClosestObjective.GetComponent<Objective>().IsComplete() && ObjectiveManager.AllCurrentActiveObjectives.Count > 0) FindClosestObjective();
        if (!ClosestObjective) return;


        if (Vector3.Distance(ClosestObjective.transform.position, transform.position) > DistanceBeforeRehint)
        {
            PauseHintCounter = false;
        }
        else
        {
            PauseHintCounter = true;
        }
        if (TimeSinceLastObjective < HintTime)
        {
            return;
        }
        //var hint = Instantiate(PuzzleHint, transform.position, Quaternion.identity);
        //hint.GetComponent<PuzzleHint>().SetTarget(ClosestObjective.transform,transform);
        //TimeSinceLastObjective = 0;
        //PauseHintCounter = true;


        //NavMeshPath path = new NavMeshPath();

        //NavMesh.FindClosestEdge(ClosestObjective.transform.position,out NavMeshHit objPos, NavMesh.AllAreas);
        //NavMesh.FindClosestEdge(transform.position, out NavMeshHit playerpos, NavMesh.AllAreas);
        //NavMesh.CalculatePath(playerpos.position, objPos.position,NavMesh.AllAreas,path);
        //renderer.positionCount = path.corners.Length;
        //renderer.SetPositions(path.corners);


        /*
        if (!ClosestObjective && ObjectiveManager.AllCurrentActiveObjectives.Count > 0) FindClosestObjective();
        if (!ClosestObjective) return;
        if (Vector3.Distance(ClosestObjective.transform.position, transform.position) > DistanceBeforeRehint)
        {
            PauseHintCounter = false;
            HintPointer.SetActive(true);
        }

        if (TimeSinceLastObjective > HintTime)
        {
            HintPointerScript.SetPlayer(transform);
            HintPointerScript.SetTarget(ClosestObjective.transform);
            HintPointer.SetActive(true);
        }*/


    }

    void FindClosestObjective()
    {
        float ClosestDistance = Mathf.Infinity;
        Objective Closest = null;
        foreach (var objective in ObjectiveManager.AllCurrentActiveObjectives)
        {
            float distance = Vector3.Distance(transform.position, objective.transform.position);
            if (distance < ClosestDistance)
            {
                ClosestDistance = distance;
                Closest = objective;
            }
        }

        ClosestObjective =  Closest;
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
        if (CurrentHealth - HealthCostPerRotationFrame * Time.deltaTime < 0 || InputManager.RotateInput == 0) return;

        //Drain mana and roatate the sun by a fixed amount based on users input
        CurrentHealth -= HealthCostPerRotationFrame * Time.deltaTime;
        TimeSinceLastDamage = 0;
        float HorizontalRotation = InputManager.RotateInput;

        Vector3 SunsRotation = SunTransform.eulerAngles;
        SunsRotation.y += HorizontalRotation * Time.deltaTime * RotationSpeed;
        SunTransform.eulerAngles = SunsRotation;
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

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.tag == "Water")
        {
            OnDeath?.Invoke();
        }
    }
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
        CurrentHealth = MaxHealth;
        controller.enabled = false;
        //Play Dead Animation
        Animator.SetBool("IsDead",true);
        StartCoroutine(PlayTransitionAfterDeath());
    }

    public IEnumerator PlayTransitionAfterDeath()
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
        OnRespawn();
    }

    void OnRespawn()
    {
        controller.enabled = true;
        Dead=false;
    }

    #endregion
}
