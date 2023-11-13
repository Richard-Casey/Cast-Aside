//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;
//using TMPro;

//public class KeyRebinder : MonoBehaviour
//{
//    public InputActionAsset inputActions;
//    public string actionMapName = "Player";
//    public string actionName = "Move";
//    public TextMeshProUGUI bindingDisplayText;
//    public Button rebindButton;

//    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
//    private int bindingIndex;

//    private void Start()
//    {
//        LoadBinding();
//        UpdateBindingDisplay();
//        rebindButton.onClick.AddListener(StartRebind);
//    }

//    public void StartRebind()
//    {
//        var actionMap = inputActions.FindActionMap(actionMapName);
//        if (actionMap == null) return;

//        var action = actionMap.FindAction(actionName);
//        if (action == null) return;

//        rebindingOperation = action.PerformInteractiveRebinding()
//            .WithControlsExcluding("Mouse")
//            .OnMatchWaitForAnother(0.1f)
//            .OnComplete(operation => RebindComplete(operation))
//            .Start();
//    }

//    private void RebindComplete(InputActionRebindingExtensions.RebindingOperation operation)
//    {
//        SaveBinding();
//        operation.Dispose();
//        UpdateBindingDisplay();
//    }

//    private void SaveBinding()
//    {
//        var action = inputActions.FindActionMap(actionMapName)?.FindAction("Move");
//        if (action != null)
//        {
//            int bindingIndex = FindBindingIndex(action, "Up");
//            if (bindingIndex >= 0)
//            {
//                string bindingOverride = action.bindings[bindingIndex].overridePath;
//                PlayerPrefs.SetString("Up_binding", bindingOverride);
//                PlayerPrefs.Save();
//            }
//        }
//    }

//    private void LoadBinding()
//    {
//        var action = inputActions.FindActionMap(actionMapName)?.FindAction("Move");
//        if (action != null)
//        {
//            int bindingIndex = FindBindingIndex(action, "Up");
//            if (bindingIndex >= 0)
//            {
//                string bindingOverride = PlayerPrefs.GetString("Up_binding", "");
//                if (!string.IsNullOrEmpty(bindingOverride))
//                {
//                    action.ApplyBindingOverride(bindingIndex, bindingOverride);
//                }
//            }
//        }
//    }

//    private void UpdateBindingDisplay()
//    {
//        var action = inputActions.FindActionMap(actionMapName)?.FindAction("Move");
//        if (action != null)
//        {
//            int bindingIndex = FindBindingIndex(action, "Up");
//            if (bindingIndex >= 0)
//            {
//                string currentBinding = action.bindings[bindingIndex].effectivePath;
//                bindingDisplayText.text = ExtractKeyFromBindingPath(currentBinding);
//            }
//        }
//    }

//    private int FindBindingIndex(InputAction action, string bindingName)
//    {
//        for (int i = 0; i < action.bindings.Count; i++)
//        {
//            if (action.bindings[i].name == bindingName)
//            {
//                return i;
//            }
//        }
//        return -1; // Not found
//    }

//    private string ExtractKeyFromBindingPath(string bindingPath)
//    {
//        // This method will extract the key name from the binding path
//        if (string.IsNullOrEmpty(bindingPath))
//        {
//            return "";
//        }

//        // Example: "<Keyboard>/w" -> "w"
//        int startIndex = bindingPath.LastIndexOf('/') + 1;
//        return bindingPath.Substring(startIndex).Replace("[Keyboard]", "").Trim();
//    }

//}


