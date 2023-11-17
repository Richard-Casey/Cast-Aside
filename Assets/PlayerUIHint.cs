using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIHint : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;

    public void SetText(string textToSet)
    {
        Text.enabled = true;
        Text.text = textToSet;
    }

    public void ClearText()
    {
        Text.text = "";
        Text.enabled = false;
    }
}
