using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using UnityEngine.Events;

public class MoveToPosition : MonoBehaviour
{

    [SerializeField] Ease MovementType = Ease.Linear;
    [SerializeField] float Duration = 2f;
    [SerializeField] Vector3 TargetPosition;
    [SerializeField] UnityEvent OnCompletion = new UnityEvent();
    Vector3 InitalPosition;
    [SerializeField] bool ForceMove = false;

    void Start()
    {
        InitalPosition = transform.localPosition;
    }

    void Update()
    {
        if (ForceMove)
        {
            DoMove();
            ForceMove = false;
        }
    }

    public void DoMove()
    {
        transform.DOLocalMove(TargetPosition, Duration).SetEase(MovementType).OnComplete(OnCompletion.Invoke);
    }

    public void ResetPosition()
    {
        transform.DOLocalMove(InitalPosition, Duration).SetEase(MovementType).OnComplete(OnCompletion.Invoke);
    }

}
