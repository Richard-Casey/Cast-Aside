using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Glyph : MonoBehaviour
{
    Vector3 initalPosition;
    bool CurrentlyHeld = false;
    bool isPlayerColliding = false;
    public GlpyhPuzzle puzzleManager;
    void OnTriggerEnter(Collider collision)
    {
        isPlayerColliding = true;
        InputManager.Interaction.AddListener(OnInteraction);
    }

    void OnTriggerExit(Collider collision)
    {
        isPlayerColliding = false;
        InputManager.Interaction.RemoveListener(OnInteraction);
    }

    void Start()
    {
        initalPosition = transform.position;
    }

    void OnInteraction(GameObject Player)
    {
        if(puzzleManager.IsPuzzleReseting) return;

        //Pick up
        if (!isInDropOff && isPlayerColliding && !CurrentlyHeld)
        {
            CurrentlyHeld = true;
            GlpyhPuzzle.OnGlyphPickup?.Invoke(gameObject, Player.transform);
        }
        //Drop
        else if (!isInDropOff&&isPlayerColliding && CurrentlyHeld && !puzzleManager.PlayerInDropOff)
        {
            CurrentlyHeld = false;
            transform.DOMove(initalPosition, 2f);
            GlpyhPuzzle.OnGlyphDrop?.Invoke(gameObject, Player.transform);
        }
        //Place in holder
        else if (isPlayerColliding && CurrentlyHeld && puzzleManager.PlayerInDropOff)
        {
            isInDropOff = true;
        }
    }

    public bool isInDropOff = false;

    public void ResetPosition()
    {
        isInDropOff = false;
        transform.DOMove(initalPosition,1f).OnComplete(() => puzzleManager.IsPuzzleReseting = false);
    }
}
