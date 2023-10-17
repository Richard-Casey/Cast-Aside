using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float HeightDisplacment = -.2f;
    [SerializeField] float PlateSpeed = 1f;
    [SerializeField] public UnityEvent<GameObject> OnPlateActivate = new UnityEvent<GameObject>();
    [SerializeField] UnityEvent OnPlateStay = new UnityEvent();
    [SerializeField] UnityEvent OnPlateLeave = new UnityEvent();

    AudioSource audio;

    bool isTransitioning = false;
    Vector3 _StartPosition = Vector3.zero;
    void OnTriggerEnter(Collider collision)
    {
        if (!audio) audio = GetComponent<AudioSource>();

        if (isTransitioning) return;

        _StartPosition = transform.position;
        StartCoroutine(Down());

        audio.Play(0);
        OnPlateActivate?.Invoke(gameObject);

    }

    void OnTriggerStay(Collider collision)
    {
        OnPlateStay?.Invoke();
    }

    void OnTriggerExit()
    {
        if (isTransitioning) return;
        OnPlateLeave?.Invoke();
        _StartPosition = transform.position;
        StartCoroutine(Up());

        audio.Play(0);
    }

    IEnumerator Up()
    {

        if (transform.position.y < _StartPosition.y - HeightDisplacment)
        {
            isTransitioning = true;
            transform.position = Vector3.MoveTowards(transform.position,_StartPosition - new Vector3(0,HeightDisplacment,0),Time.deltaTime * PlateSpeed);
            yield return null;
        }

        isTransitioning = false;
    }

    IEnumerator Down()
    {

        if (transform.position.y > _StartPosition.y + HeightDisplacment)
        {
            isTransitioning = true;
            transform.position = Vector3.MoveTowards(transform.position, _StartPosition + new Vector3(0, HeightDisplacment, 0), Time.deltaTime * PlateSpeed);
            yield return null;
        }

        isTransitioning = false;

    }
}
