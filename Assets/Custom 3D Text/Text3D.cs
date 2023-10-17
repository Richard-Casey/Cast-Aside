using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Text3D : MonoBehaviour
{
    [SerializeField] List<GameObject> NumberPrefabs = new List<GameObject>();
    [SerializeField] public string NumberToDisplay = "9999";
    [SerializeField] float Spacing = 1f;
    [SerializeField] bool CenterAlignText = true;
    [SerializeField] bool ShouldBeRandomNumber = false;
    [SerializeField] int RandomNumberDigits = 4;
    [SerializeField] int RandomDigitLimit = 4;
    [SerializeField] Material DefaultMaterial;
    [SerializeField] bool _shouldSpawnAtTransforms = false;
    [SerializeField] List<Transform> spawnTransforms = new List<Transform>();


    List<GameObject> CurrentlyDisplayedNumber = new List<GameObject>();

    void Start()
    {
        DefaultMaterial = new Material(DefaultMaterial);
        if (ShouldBeRandomNumber)
        {
            DisplayNumber(NumberToDisplay);
        } else
        {
            DisplayRandom(RandomNumberDigits);
        }
    }

    void DisplayRandom(int Digits)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Digits; i++)
        {
            sb.Append(Random.Range(0, RandomDigitLimit));
        }

        NumberToDisplay = sb.ToString();
        DisplayNumber(NumberToDisplay);
    }

    void DisplayNumber(string Number)
    {
        List<int> Numbers = new List<int>();
        foreach (var character in Number)
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
            GameObject newNumber;
            if (_shouldSpawnAtTransforms && spawnTransforms.Count > i)
            {
                newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                    spawnTransforms[i].position, spawnTransforms[i].rotation);
            }
            else
            {
                 newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                    transform.position - new Vector3(halfWidth + (i * Spacing), 0, 0), Quaternion.identity);
            }
            CurrentlyDisplayedNumber.Add(newNumber);
            if (DefaultMaterial) newNumber.GetComponent<MeshRenderer>().material = DefaultMaterial;
            newNumber.transform.parent = transform;
        }
    }
}
