using UnityEngine;
using UnityEngine.EventSystems;

public class PrefabSelector : MonoBehaviour, IPointerClickHandler
{
    public Customisation customisationScript; // Reference to the Customisation script
    public int index; // This should be set in the Unity Editor for each prefab

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PrefabSelector clicked with index: " + index);
        customisationScript.SelectPrefab(index);
    }



}