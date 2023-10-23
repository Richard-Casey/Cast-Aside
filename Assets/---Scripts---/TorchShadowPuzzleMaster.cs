using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorchShadowPuzzleMaster : MonoBehaviour
{
    [SerializeField] [Tooltip("The angle at which the far-most torch can spawn")] [Range(0,180)] float Degrees = 180f;
    [SerializeField] int NumberOfTorches = 6;
    [SerializeField] Text3D NumberDisplay;
    [SerializeField] float DistanceOfTorchFromCenter = 5f;
    [SerializeField] GameObject TorchPrefab;
    Dictionary<GameObject, Torch> TorchesInPuzzle = new Dictionary<GameObject, Torch>();
    List<(float, int)> AllDisplayedNumbersWithXPos = new List<(float, int)>();
    List<int> Soloution = new List<int>();
    bool isPuzzleActive = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateTorches();
        NumberDisplay.DisplayRandomUnique(NumberOfTorches);

        FindOrder();
    }

    //Sorts all numbers by there x position relative to the center of the puzzle
    void FindOrder()
    {
        foreach (var Tupple in NumberDisplay.CurrentlyDisplayedNumber)
        {
            AllDisplayedNumbersWithXPos.Add((Tupple.Item1.transform.localPosition.x,Tupple.Item2));
        }

        AllDisplayedNumbersWithXPos = AllDisplayedNumbersWithXPos.OrderByDescending(x => x.Item1).ToList();
        AllDisplayedNumbersWithXPos.Reverse();

        foreach (var Tupple in AllDisplayedNumbersWithXPos)
        {
            Soloution.Add(Tupple.Item2);
        }
        
    }

    //Creates a torch in a spherical radius around the player starting at x degrees to the left rotating round to right
    void CreateTorches()
    {
        float DegreesBetweenTorch = Degrees*2f / (NumberOfTorches);
        

        for (int i = 1; i < NumberOfTorches + 1; i++)
        {
            float degreesOnCircle = -Degrees + (i * DegreesBetweenTorch);
            float radians = degreesOnCircle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            Vector3 Position = transform.position + new Vector3(x * DistanceOfTorchFromCenter, 0, z * DistanceOfTorchFromCenter);
            GameObject Torch = Instantiate(TorchPrefab, Position, transform.rotation, transform);
            TorchesInPuzzle.Add(Torch,Torch.GetComponent<Torch>());
        }

    }

    void OnTriggerEnter()
    {
        isPuzzleActive = true;
    }

    void OnTriggerExit()
    {
        isPuzzleActive = false;
    }

}
