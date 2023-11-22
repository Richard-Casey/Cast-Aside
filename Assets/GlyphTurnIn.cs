using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlyphTurnIn : MonoBehaviour
{
    [SerializeField] int ID;
    
    bool isPlayerInCollider = false;

    [SerializeField] public Transform GlyphHolderPosition;
    [SerializeField] GlpyhPuzzle manager;


    void Start()
    {
        InputManager.Interaction.AddListener(OnPlayerInteraction);
    }

    void OnPlayerInteraction(GameObject Player)
    {
        if (isPlayerInCollider)
        {
            GlpyhPuzzle.OnGlyphPlace?.Invoke(ID);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        isPlayerInCollider = true;
        manager.PlayerInDropOff = true;
        
    }

    void OnTriggerExit(Collider collider)
    {
        isPlayerInCollider = false;
        manager.PlayerInDropOff = false;
    }

}
