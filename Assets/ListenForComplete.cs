using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ListenForComplete : MonoBehaviour
{
    [SerializeField] GameObject welldonetext;

    public void Start()
    {
        ObjectiveManager.AllObjectivesComplete?.AddListener(OnComplete);
    }

    public void OnComplete()
    {
        welldonetext.SetActive(true);
        welldonetext.transform.DOShakePosition(.4f, Vector3.one);
        StartCoroutine(Hide(5f));
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        welldonetext.SetActive(false);
    }
}
