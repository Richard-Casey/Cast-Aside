using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrefabSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Customisation customisationScript; // Reference to the Customisation script
    public int index; // This should be set in the Unity Editor for each prefab
    public Image prefabImage;

    private void Start()
    {
        customisationScript.OnPrefabSelected += UpdateAlphaBasedOnSelection;

        // Initialize the alpha based on whether this prefab is the currently selected one
        if (index == customisationScript.GetCurrentPlayerIndex())
        {
            SetAlpha(255);
        }
        else
        {
            SetAlpha(180);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PrefabSelector clicked with index: " + index);
        customisationScript.SelectPrefab(index);
        SetAlpha(255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (index != customisationScript.GetCurrentPlayerIndex())
        {
            SetAlpha(180);
        }
    }

    private void SetAlpha(byte alpha)
    {
        Color color = prefabImage.color;
        color.a = alpha / 255f;
        prefabImage.color = color;
    }

    private void OnDestroy()
    {
        customisationScript.OnPrefabSelected -= UpdateAlphaBasedOnSelection;
    }

    private void UpdateAlphaBasedOnSelection(int selectedIndex)
    {
        if (index == selectedIndex)
        {
            SetAlpha(255);
        }
        else
        {
            SetAlpha(180);
        }
    }
}