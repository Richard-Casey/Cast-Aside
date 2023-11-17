using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//This script is temp
public class SpeechBubble : MonoBehaviour
{
    public List<string> DialougeToSay = new List<string>();
    public string EmptyDialougeMessage = "Hey Ive Had Enough Of You Go Away....";


    public TextMeshProUGUI textMeshPro;
    int CurrentText = 0;

    void Start()
    {
        textMeshPro.text = DialougeToSay[CurrentText];
    }

    public void FixedUpdate()
    {
    }

    public void ShowNextText()
    {
        CurrentText++;
        if (CurrentText > DialougeToSay.Count - 1)
        {
            textMeshPro.text = EmptyDialougeMessage;
        }
        else
        {
            textMeshPro.text = DialougeToSay[CurrentText];
        }
    }
}
