using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform StartingPosition;
    [SerializeField] float TransitionSpeed = 1f;
    [SerializeField] float TransitionHeight = 5f;
    [SerializeField] Transform TestTarget;

    Transform CurrentTarget;
    float CurrentTime = 0;

    public void Start()
    {
        CurrentTarget = StartingPosition;
        StartTransition(TestTarget);
    }

    public void StartTransition(Transform Target)
    {
        StartCoroutine(TransitionTo(Target));
    }

    IEnumerator TransitionTo(Transform Target)
    {
        while (transform.position.y < CurrentTarget.position.y + TransitionHeight)
        {
            transform.position = Vector3.Lerp(CurrentTarget.position, CurrentTarget.position + new Vector3(0,TransitionHeight,0), CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        CurrentTime = 0;
        transform.position = Target.position + new Vector3(0, TransitionHeight, 0);
        StartCoroutine(TransitionFrom(Target));
    }

    IEnumerator TransitionFrom(Transform Target)
    {
        while (transform.position.y > Target.position.y)
        {
            transform.position = Vector3.Slerp(Target.position+ new Vector3(0, TransitionHeight, 0), Target.position , CurrentTime);
            CurrentTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }
        CurrentTime = 0;
    }
}
