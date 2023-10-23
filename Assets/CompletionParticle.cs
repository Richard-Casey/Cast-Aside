using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CompletionParticle : MonoBehaviour
{
    [SerializeField] float RaiseDistance;
    [SerializeField] float MaxSize = 5f;
    [SerializeField] float RaiseTime = 1f;
    [SerializeField] float MoveToTargetTime = 5f;

    Vector3 StartingPos;
    Vector3 TargetPos;
    ParticleSystem.MainModule psMain;
    void Start()
    {
        StartTransition(Vector3.one, Color.green);
    }

    public void StartTransition(Vector3 TargetPos, Color ParticleColor)
    {
        StartingPos = transform.position;
        this.TargetPos = TargetPos;
        RaiseUp();
    }

    void RaiseUp()
    {
        
        transform.DOMoveY(StartingPos.y + RaiseDistance, RaiseTime).SetEase(Ease.InOutCubic).OnComplete(MoveTo);
        transform.DOShakeRotation(RaiseTime, Vector3.one * 10f);
        /*DOVirtual.Float(0, MaxSize, RaiseTime, (value) =>
        {
            transform.localScale = Vector3.one * value;
        });*/
    }

    void MoveTo()
    {
        transform.DOMove(TargetPos, MoveToTargetTime, false).SetEase(Ease.InOutCirc);
        
    }
}
