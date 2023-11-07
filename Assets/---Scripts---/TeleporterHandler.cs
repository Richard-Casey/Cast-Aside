using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TeleporterHandler : MonoBehaviour
{
    [SerializeField] Transform TargetPoint;
    [SerializeField] float HalfSceneTransitionLength = .5f;

    public void OnTriggerEnter(Collider collider)
    {
        StartTeleport(collider.transform);
    }

    public void StartTeleport(Transform player){
    SceneTransitions.PlayTransition?.Invoke(SceneTransitions.AnimationsTypes.CircleSwipe);
    StartCoroutine(Teleport(player));
    }


IEnumerator Teleport(Transform player)
{
    yield return new WaitForSeconds(HalfSceneTransitionLength);
    player.position = TargetPoint.position;
}

}