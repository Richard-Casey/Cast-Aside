using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Gate : MonoBehaviour
{
    [SerializeField] Vector3 OpenOffset = new Vector3(0, 1, 0);
    [SerializeField] float TransitionTime = 1f;
    [SerializeField] AudioSource audioSource;


    bool isOpen = false;
    bool isTransitioning = false;

    public void Interact()
    {
        transform.DOShakePosition(TransitionTime, Vector3.one * .1f,5,90,false,true,ShakeRandomnessMode.Harmonic);
        audioSource.Play();
        isTransitioning = true;

        switch (isOpen)
        {
            case true:
                transform.DOMoveY(transform.position.y - OpenOffset.y, TransitionTime, false).OnComplete(() => isTransitioning = false);
                break;
            case false:
                transform.DOMoveY(transform.position.y + OpenOffset.y, TransitionTime, false).OnComplete(() => isTransitioning = false);
                break;
        }

        isOpen = !isOpen;
    }

}
