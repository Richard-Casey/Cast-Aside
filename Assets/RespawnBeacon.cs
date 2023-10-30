using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBeacon : MonoBehaviour
{
    [SerializeField] Transform RespawnPoint;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameSpecificCharacterController playerCharacterController;
            if (collider.TryGetComponent<GameSpecificCharacterController>(out playerCharacterController))
            {
                playerCharacterController.SetSpawnPoint(RespawnPoint);
            }
            
        }
    }
}
