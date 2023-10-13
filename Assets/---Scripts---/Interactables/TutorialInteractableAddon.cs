using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class TutorialInteractableAddon : MonoBehaviour
{
    public void DisplayTutorial(string textToDisplay)
    {
        TutorialMessage.DisplayTutorialMessage?.Invoke(textToDisplay);
    }

    public void ClearTutorial()
    {
        TutorialMessage.HideTutorialMessage?.Invoke();
    }
}
