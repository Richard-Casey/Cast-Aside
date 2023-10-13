using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3D : MonoBehaviour
{
    [SerializeField] List<GameObject> NumberPrefabs = new List<GameObject>();
    [SerializeField] float NumberToDisplay = 9999;
    [SerializeField] float Spacing = 1f;
    [SerializeField] bool CenterAlignText = true;
    [SerializeField] Material DefaultMaterial;


    List<GameObject> CurrentlyDisplayedNumber = new List<GameObject>();

    void Start()
    {
        DefaultMaterial = new Material(DefaultMaterial);
        DisplayNumber(NumberToDisplay);
    }

    void DisplayNumber(float Number)
    {
        string chars = Number.ToString();
        List<int> Numbers = new List<int>();
        foreach (var character in chars)
        {
            int numberfromcharacter;
            if (int.TryParse(character.ToString(),out numberfromcharacter))
            {
                Numbers.Add(numberfromcharacter);
            }
        }

        float TotalWidth = (Numbers.Count - 1) * Spacing;
        float halfWidth = TotalWidth / 2f;

        for (int i = 0 ; i < Numbers.Count ; i++)
        {
            GameObject newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                transform.position - new Vector3(halfWidth + (i * Spacing), 0, 0), Quaternion.identity);
            CurrentlyDisplayedNumber.Add(newNumber);
            if (DefaultMaterial) newNumber.GetComponent<MeshRenderer>().material = DefaultMaterial;
            newNumber.transform.parent = transform;
        }
    }
}
