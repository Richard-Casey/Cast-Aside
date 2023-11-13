//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using TMPro;

//public class ControlMapper : MonoBehaviour
//{
//    private Dictionary<string, KeyCode> controlMappings;
//    private bool waitingForKey = false;
//    private string currentAction = "";

//    private void Start()
//    {
//        controlMappings = new Dictionary<string, KeyCode>();
//        // Initialize with default controls
//        controlMappings["Up"] = KeyCode.W;
//        controlMappings["Down"] = KeyCode.S;
//        controlMappings["Left"] = KeyCode.A;
//        controlMappings["Right"] = KeyCode.D;
//        controlMappings["Jump"] = KeyCode.Space;
//        controlMappings["Sprint"] = KeyCode.LeftShift;
//        controlMappings["Interact"] = KeyCode.F;

//        foreach (var mapping in controlMappings)
//        {
//            UpdateControlText(mapping.Key, mapping.Value);
//        }
//    }

//    // Method to update a control
//    public void UpdateControl(string action, KeyCode newKey)
//    {
//        if (controlMappings.ContainsKey(action))
//        {
//            controlMappings[action] = newKey;
//            UpdateControlText(action, newKey); // Update UI text
//        }
//    }


//    // Method to get the current key for an action
//    public KeyCode GetControlKey(string action)
//    {
//        if (controlMappings.TryGetValue(action, out KeyCode key))
//        {
//            return key;
//        }
//        return KeyCode.None; // Or a default key
//    }

//    private void Update()
//    {
//        if (waitingForKey)
//        {
//            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
//            {
//                if (Input.GetKeyDown(kcode))
//                {
//                    UpdateControl(currentAction, kcode);
//                    waitingForKey = false;
//                    break;
//                }
//            }
//        }
//    }

//    public void StartKeyChange(string action)
//    {
//        currentAction = action;
//        waitingForKey = true;
//    }

//    private void SaveControls()
//    {
//        foreach (var mapping in controlMappings)
//        {
//            PlayerPrefs.SetString(mapping.Key, mapping.Value.ToString());
//        }
//        PlayerPrefs.Save();
//    }

//    private void LoadControls()
//    {
//        foreach (var mapping in controlMappings.Keys.ToList())
//        {
//            if (PlayerPrefs.HasKey(mapping))
//            {
//                KeyCode storedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(mapping));
//                controlMappings[mapping] = storedKey;
//            }
//        }
//    }


//    public void UpdateControlText(string action, KeyCode newKey)
//    {
//        TMP_Text actionText = GameObject.Find(action + "Text").GetComponent<TMP_Text>();
//        if (actionText != null)
//        {
//            actionText.text = action + ": " + newKey.ToString();
//        }
//        else
//        {
//            Debug.LogError("Text object not found for action: " + action);
//        }
//    }

//}