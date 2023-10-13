using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;


public class TutorialMessage : MonoBehaviour
{
    public static UnityEvent<string> DisplayTutorialMessage = new UnityEvent<string>();
    public static UnityEvent HideTutorialMessage = new UnityEvent();

    [SerializeField] TextMeshProUGUI TextField;

    public void Start()
    {
        DisplayTutorialMessage.AddListener(DisplayText);
        HideTutorialMessage.AddListener(HideText);
    }
    
    public void OnDestroy()
    {
        DisplayTutorialMessage.RemoveListener(DisplayText);
        HideTutorialMessage.RemoveListener(HideText);
    }

    void DisplayText(string textToDisplay)
    {
        if (TextField)
        {
            TextField.enabled = true;
            TextField.text = textToDisplay;
        }
    }

    void HideText()
    {
        TextField.text = "";
        TextField.enabled = false;
    }
}
