using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebinderNEW : MonoBehaviour
{
    public TMP_Text currentKeyText;

    [SerializeField]
    private InputActionReference playerInput;

    private string originalBinding;

    public void StartRebinding()
    {
        Debug.Log("StartRebinding called");
        currentKeyText.text = "Current Key: ...";

        playerInput.action.Disable();

        foreach (var binding in playerInput.action.bindings)
        {
            Debug.Log("Binding: " + binding.path);
        }


        int bindingIndex = FindBindingIndexByPath("<Keyboard>/w");
        if (bindingIndex != -1)
        {
            var rebindOperation = playerInput.action.PerformInteractiveRebinding(bindingIndex)
                .WithTimeout(5f)
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete(operation))
                .Start();
        }
        else
        {
            Debug.LogError("Failed to find binding index for Up");
        }
    }


    private void RebindComplete(UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation operation)
    {
        Debug.Log("Rebind operation completed");
        operation.Dispose();

        // Use FindBindingIndexByPath instead of FindBindingIndex
        int bindingIndex = FindBindingIndexByPath("<Keyboard>/w"); // Replace with the correct path if needed
        if (bindingIndex != -1)
        {
            string newBinding = playerInput.action.bindings[bindingIndex].ToDisplayString(); // Get the display string of the new binding
            Debug.Log("New binding: " + newBinding);
            currentKeyText.text = "Current Key: " + newBinding;
        }
        else
        {
            Debug.LogError("Failed to find updated binding index for Up");
        }

        playerInput.action.Enable();
    }


    private void RebindCancelled(UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation operation)
    {
        Debug.Log("Rebind operation cancelled");
        operation.Dispose();

        // Restore the original binding
        currentKeyText.text = "Current Key: " + originalBinding;

        playerInput.action.Enable();
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("Key Pressed: " + Keyboard.current.anyKey.ReadValue());
        }
    }

    private int FindBindingIndexByPath(string bindingPath)
    {
        var bindings = playerInput.action.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].isPartOfComposite && bindings[i].path == bindingPath)
            {
                Debug.Log("Found binding index for path " + bindingPath + ": " + i);
                return i;
            }
        }
        Debug.LogError("Binding not found for path: " + bindingPath);
        return -1; // Return -1 if not found
    }


}
