using System.Collections;
using UnityEngine;

public class WaypointRunner : MonoBehaviour
{
    public Transform[] corridorWaypoints;
    public Transform centerWaypoint;
    public float speed = 5.0f;
    public float pauseTime = 1.0f;
    private Transform currentTarget;
    private bool isPaused = false;
    private Animator animator;

    void Start()
    {
        // Dynamically find and assign the waypoints
        Transform roomTransform = GameObject.Find("MainMenuFountain").transform;
        Transform triggerPointsTransform = roomTransform.Find("TriggerPoints");

        corridorWaypoints = new Transform[4];
        corridorWaypoints[0] = triggerPointsTransform.Find("North");
        corridorWaypoints[1] = triggerPointsTransform.Find("South");
        corridorWaypoints[2] = triggerPointsTransform.Find("West");
        corridorWaypoints[3] = triggerPointsTransform.Find("East");

        // Assign centerWaypoint
        centerWaypoint = triggerPointsTransform.Find("Center");

        animator = GetComponent<Animator>();

        int randomIndex = Random.Range(0, corridorWaypoints.Length);

        Vector3 spawnPosition = corridorWaypoints[randomIndex].position;
        spawnPosition.y += 1;

        //transform.position = corridorWaypoints[randomIndex].position;

        transform.position = spawnPosition;

        currentTarget = centerWaypoint;
    }

    void Update()
    {
        if (isPaused)
        {
            animator.SetBool("IsRunning", false); 
            return;
        }

        
        float step = speed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, currentTarget.position, step);
        transform.position = newPosition;

        
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = lookRotation;
        }

        
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            animator.SetBool("IsRunning", false);

            if (currentTarget == centerWaypoint)
            {
                // At the center, pause for a moment
                StartCoroutine(PauseAtCenter());
            }
            else
            {
                // At The corridor
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, corridorWaypoints.Length);
                } while (currentTarget == corridorWaypoints[randomIndex]);

                transform.position = corridorWaypoints[randomIndex].position;

                // Next target is the center
                currentTarget = centerWaypoint;
            }
        }
        else
        {
            animator.SetBool("IsRunning", true); // Play running animation
        }
    }

    IEnumerator PauseAtCenter()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;

        // Pick a random corridor 
        int randomIndex = Random.Range(0, corridorWaypoints.Length);
        currentTarget = corridorWaypoints[randomIndex];
    }

    public void SetInitialState(Transform target, bool pauseState)
    {
        currentTarget = target;
        isPaused = pauseState;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }

    public bool IsPaused()
    {
        return isPaused;
    }


}
