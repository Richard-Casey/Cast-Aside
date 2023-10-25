using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Text3D : MonoBehaviour
{
    
    [SerializeField] List<GameObject> NumberPrefabs = new List<GameObject>();
    [SerializeField] public string NumberToDisplay = "9999";

    [SerializeField] float Spacing = 1f;
    [SerializeField] float RandomSpawningRadius = 5f;
    [SerializeField] bool CenterAlignText = true;
    [SerializeField] bool ShouldBeRandomNumber = false;
    [SerializeField] int RandomNumberDigits = 4;
    [SerializeField] int RandomDigitLimit = 4;
    [SerializeField] Vector3 NumberScale = new Vector3(1, 1, 1);
    [SerializeField] Material DefaultMaterial;
    [SerializeField] bool _shouldSpawnAtTransforms = false;
    [SerializeField] bool _shouldSpawnRandomly = false;
    [SerializeField] bool _shouldAutoSpawn = true;
    [SerializeField] List<Transform> spawnTransforms = new List<Transform>();
    

    public List<(GameObject,int)> CurrentlyDisplayedNumber = new List<(GameObject, int)>();
    
    void Start()
    {
        DefaultMaterial = new Material(DefaultMaterial);
        if (_shouldAutoSpawn)
        {
            if (ShouldBeRandomNumber)
            {
                DisplayNumber(NumberToDisplay);
            }
            else
            {
                DisplayRandom(RandomNumberDigits);
            }
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

    public List<int> GetSolution()
    {
        List<int> Return = new List<int>();
        foreach (var tuuple in CurrentlyDisplayedNumber)
        {
            Return.Add(tuuple.Item2);
        }

        return Return;
    }

    public void DisplayRandomUnique(int Digits)
    {
        List<int> digits = new List<int>();

        for (int i = 0; i < Digits; i++)
        {
            digits.Add(i);
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Digits && i < 10; i++)
        {
            int index = Random.Range(0, digits.Count);
            sb.Append(digits[index]);
            digits.RemoveAt(index);
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

        float SpawningStep = (RandomSpawningRadius * 2f) / Numbers.Count;

        for (int i = 0 ; i < Numbers.Count ; i++)
        {
            GameObject newNumber;
            if (_shouldSpawnAtTransforms && spawnTransforms.Count > i)
            {
                newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                    spawnTransforms[i].position, spawnTransforms[i].rotation);
                newNumber.transform.localScale = NumberScale;
            }
            else
            {
                if (_shouldSpawnRandomly)
                {
                    newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                        transform.position + new Vector3(-RandomSpawningRadius + (i* SpawningStep),1, Random.Range(-RandomSpawningRadius, RandomSpawningRadius)), Quaternion.LookRotation(Vector3.up,transform.right));
                    newNumber.transform.localScale = NumberScale;
                }
                else
                {
                    newNumber = Instantiate(NumberPrefabs[Numbers[i]],
                        transform.position - new Vector3(halfWidth + (i * Spacing), 0, 0), Quaternion.identity);
                    newNumber.transform.localScale = NumberScale;
                }
            }
            CurrentlyDisplayedNumber.Add((newNumber,Numbers[i]));
            if (DefaultMaterial) newNumber.GetComponent<MeshRenderer>().material = DefaultMaterial;
            newNumber.transform.parent = transform;
        }
    }
}
