using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{

    [SerializeField] float _interactionDistance = 1f;
    [SerializeField] UnityEvent<GameObject> _onEnterInteractionArea;
    [SerializeField] protected UnityEvent<GameObject> OnInteraction;
    [SerializeField] UnityEvent<GameObject> _onStay;
    [SerializeField] UnityEvent<GameObject> _onLeaveInteractionArea;

    [ExecuteInEditMode]
    void Awake()
    {
        SphereCollider collider;
        if (!gameObject.TryGetComponent<SphereCollider>(out collider))
        {
            SphereCollider thisCollider = gameObject.AddComponent<SphereCollider>();
            thisCollider.isTrigger = true;
        }

        collider.isTrigger = true;
        collider.radius = _interactionDistance;
    }


    void OnTriggerEnter(Collider collider)
    {
        _onEnterInteractionArea.Invoke(collider.gameObject);
        InputManager.Interaction.AddListener(OnInteraction.Invoke);
    }

    void OnTriggerStay(Collider collider)
    {
        _onStay?.Invoke(collider.gameObject);
    }

    void OnTriggerExit(Collider collider)
    {
        InputManager.Interaction.RemoveListener(OnInteraction.Invoke);
        _onLeaveInteractionArea?.Invoke(collider.gameObject);
    }

    public void CreateGameObject(GameObject GO)
    {
        Instantiate(GO);
    }
}
