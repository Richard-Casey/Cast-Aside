//using UnityEngine;
//using UnityEngine.InputSystem;
//using TMPro;
//using UnityEngine.UI;

//public class ControlsUIManager : MonoBehaviour
//{
//    public Transform contentPanel;
//    public GameObject rowPrefab;
//    public PlayerInput playerInput;

//    void Start()
//    {
//        PopulateControlsList();
//    }

//    void PopulateControlsList()
//    {
//        foreach (var action in playerInput.actions)
//        {
//            GameObject newRow = Instantiate(rowPrefab, contentPanel);
//            TextMeshProUGUI movementText = newRow.transform.Find("Movement").GetComponent<TextMeshProUGUI>();
//            TextMeshProUGUI keyText = newRow.transform.Find("Key").GetComponent<TextMeshProUGUI>();
//            Button rebindButton = newRow.transform.Find("KeybindButton").GetComponent<Button>();

//            movementText.text = action.name;
//            keyText.text = action.GetBindingDisplayString(); // This gets the current bound key

//            rebindButton.onClick.AddListener(() => StartRebind(action.name, newRow));
//        }
//    }

//    public void StartRebind (string actionName, GameObject row)
//    {

//    }
  
//}
