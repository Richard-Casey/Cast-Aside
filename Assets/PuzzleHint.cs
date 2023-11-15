using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PuzzleHint : MonoBehaviour
{
    [SerializeField] NavMeshAgent ThisAgent;
    [SerializeField] ParticleSystem thisParticleSystem;
    Transform Player;
    Transform Target;
    [SerializeField] float DistanceFromPlayer;
    [SerializeField] float DistanceFromTarget;
    [SerializeField] float MaxDistanceFromPlayer;
    [SerializeField] float CorrectionSpeed = 1f;

    public void SetTarget(Transform Target , Transform Player)
    {
        if (ThisAgent )
        {
            if (!ThisAgent.isOnNavMesh) return;
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
            Vector3 Direction = (Player.position - transform.position).normalized;
            ThisAgent.Move(Direction * CorrectionSpeed * Time.fixedDeltaTime);

        }

        float localDistanceFromTarget = Vector3.Distance(transform.position, Target.position);
        if(localDistanceFromTarget < DistanceFromTarget) OnDisappear();
    }

    public void OnDisappear()
    {
        thisParticleSystem.Stop();
        StartCoroutine(WaitForFinish());
    }

    IEnumerator WaitForFinish()
    {
        while (thisParticleSystem.IsAlive()) yield return null;
        Destroy(gameObject);
    }
}
