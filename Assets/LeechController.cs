using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class LeechController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float ModelHeight = 1f;
    [SerializeField] float BuryDuration = 1f;
    [SerializeField] float DistanceBeforeUpdatePath = 1f;

    [SerializeField] Transform ModelTransform;
    NavMeshAgent thisAgent;
    float DefaultSpeed;
    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        DefaultSpeed = thisAgent.speed;
    }


    void FixedUpdate()
    {
        Vector3 Difference = thisAgent.destination - playerTransform.position;
        if (Vector3.SqrMagnitude(Difference) > DistanceBeforeUpdatePath * DistanceBeforeUpdatePath)
        {
            thisAgent.SetDestination(playerTransform.position);
        }
        
        if(thisAgent.isOnOffMeshLink)Bury();
    }


    void Bury()
    {
        thisAgent.speed = 0f;
        ModelTransform.DOLocalMoveY(-ModelHeight, BuryDuration).SetEase(Ease.Linear).OnComplete(Transition);
        
    }

    void Transition()
    {
        thisAgent.CompleteOffMeshLink();
        RaiseUp();
    }

    void RaiseUp()
    {
        ModelTransform.DOLocalMoveY(0, BuryDuration).SetEase(Ease.Linear).OnComplete(FinishedTransition);
    }

    void FinishedTransition()
    {
        thisAgent.speed = DefaultSpeed;
    }
}
