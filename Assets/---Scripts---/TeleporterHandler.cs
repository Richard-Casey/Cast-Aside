using UnityEngine;
using UnityEngine.Events;

public class TeleporterHandler : MonoBehaviour
{
    public static UnityEvent<Transform> Teleported = new UnityEvent<Transform>();

    private Transform LastCollidedTransform;
    [SerializeField] private Transform TargetPoint;

    public void Destory()
    {
        CameraManager.TransitionToCompleted.RemoveListener(Teleport);
    }

    //[SerializeField] public CameraManager.TransitionType transitionType;
    public void OnTriggerEnter(Collider collision)
    {
        LastCollidedTransform = collision.transform;
        Teleported?.Invoke(TargetPoint);
    }

    public void Start()
    {
        CameraManager.TransitionToCompleted.AddListener(Teleport);
    }
    private void Teleport()
    {
        if (LastCollidedTransform != null)
        {
            LastCollidedTransform.position = TargetPoint.position;
        }
        LastCollidedTransform = null;
    }
}