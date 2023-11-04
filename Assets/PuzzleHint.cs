using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PuzzleHint : MonoBehaviour
{
    [SerializeField] NavMeshAgent ThisAgent;
    Transform Player;
    Transform Target;
    [SerializeField] float DistanceFromPlayer;
    [SerializeField] float DistanceFromTarget;
    [SerializeField] float MaxDistanceFromPlayer;
    public void SetTarget(Transform Target , Transform Player)
    {
        if (ThisAgent)
        {
            ThisAgent.SetDestination(Target.position);
        }

        this.Player = Player;
        this.Target = Target;
        
    }

    public void FixedUpdate()
    {
        if (!Player || !ThisAgent) return;
        
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance > DistanceFromPlayer)
        {
            ThisAgent.isStopped = true;
        }
        else ThisAgent.isStopped = false;

        if (distance > MaxDistanceFromPlayer)
        {
            var newParticle = Instantiate(gameObject, Player.position, Quaternion.identity);
            newParticle.GetComponent<PuzzleHint>().SetTarget(Target,Player);
            Destroy(gameObject);
        }

        float localDistanceFromTarget = Vector3.Distance(transform.position, Target.position);
        if(localDistanceFromTarget < DistanceFromTarget) OnDisappear();
    }

    public void OnDisappear()
    {
        Destroy(gameObject);
    }
}
