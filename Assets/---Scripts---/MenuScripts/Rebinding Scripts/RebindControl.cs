using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class KeySpritePair
{
    public string key;
    public Sprite sprite;
}

public class RebindControl : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public string actionName = "Move";
    public string bindingName = "Up";

    public Button rebindButton;
    public Image keyImage;

    [SerializeField]
    private List<KeySpritePair> keySpritePairs;

    private Dictionary<string, Sprite> keySprites;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        keySprites = new Dictionary<string, Sprite>();

        foreach (var pair in keySpritePairs)
        {
            if (!string.IsNullOrEmpty(pair.key) && pair.sprite != null)
            {
                keySprites[pair.key] = pair.sprite;
            }
        }
    }

    private void Start()
    {
        LoadBinding();
        rebindButton.onClick.AddListener(StartRebind);
        UpdateKeyImage();
    }

    public void StartRebind()
    {
        Debug.Log("StartRebind() is being called");
        var actionMap = inputActions.FindActionMap(actionMapName);
        if (actionMap == null) return;

        var action = actionMap.FindAction(actionName);
        if (action == null) return;

        rebindingOperation = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(operation))
            .Start();
    }

    private void RebindComplete(InputActionRebindingExtensions.RebindingOperation operation)
    {
        SaveBinding();
        operation.Dispose();
        UpdateKeyImage();

        Debug.Log("Rebinding complete. New binding: " + operation.selectedControl.path);
    }


    private void SaveBinding()
    {
        var actionMap = inputActions.FindActionMap(actionMapName);
        if (actionMap == null) return;

        var action = actionMap.FindAction(actionName);
        if (action == null) return;

        string bindingOverride = action.bindings[0].overridePath;
        if (!string.IsNullOrEmpty(bindingOverride))
        {
            PlayerPrefs.SetString(actionName + "_binding", bindingOverride);
            PlayerPrefs.Save();
        }

        Debug.Log("Saved Binding: " + bindingOverride);
    }

    private void LoadBinding()
    {
        var actionMap = inputActions.FindActionMap(actionMapName);
        if (actionMap == null) return;

        var action = actionMap.FindAction(actionName);
        if (action == null) return;

        string bindingOverride = PlayerPrefs.GetString(actionName + "_binding", string.Empty);
        if (!string.IsNullOrEmpty(bindingOverride))
        {
            action.ApplyBindingOverride(0, bindingOverride);
        }

        Debug.Log("Loaded binding: " + bindingOverride);
    }

    private void UpdateKeyImage()
    {
        var actionMap = inputActions.FindActionMap(actionMapName);
        if (actionMap == null) return;

        var action = actionMap.FindAction(actionName);
        if (action == null) return;

        string currentBinding = action.bindings[0].effectivePath;
        if (!string.IsNullOrEmpty(currentBinding))
        {
            string key = currentBinding.Replace("<keyboards>/", "");
            if (keySprites.TryGetValue(key, out Sprite sprite))
            {
                keyImage.sprite = sprite;
                Debug.Log("Key image updated to: " + key);
            }
            else
            {
                Debug.Log("Sprite not found for key: " + key);
            }
        }
    }

}