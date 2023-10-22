using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchShadowPuzzleMaster : MonoBehaviour
{
    [SerializeField] [Tooltip("The angle at which the far-most torch can spawn")] [Range(0,180)] float Degrees = 180f;
    [SerializeField] int NumberOfTorches = 6;
    [SerializeField] Text3D NumberDisplay;
    [SerializeField] float DistanceOfTorchFromCenter = 5f;
    [SerializeField] GameObject TorchPrefab;
    Dictionary<GameObject, Torch> TorchesInPuzzle = new Dictionary<GameObject, Torch>();
    int Soloution;
    bool isPuzzleActive = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateTorches();
        NumberDisplay.DisplayRandomUnique(NumberOfTorches);
        Soloution = int.Parse(NumberDisplay.NumberToDisplay);
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
            Instantiate(TorchPrefab, Position, transform.rotation, transform);
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
