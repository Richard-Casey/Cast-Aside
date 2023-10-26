using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorchShadowPuzzleMaster : MonoBehaviour
{
    [SerializeField] public Transform ShadowCaster;
    [SerializeField] public LayerMask shadowCasterLayerMask;
    [SerializeField] public Transform sunTransform;
    [SerializeField] public float TorchSensitivity;
    public float TorchTimeNeededInShadowToGoOut = 3f;
    private List<(float, int)> AllDisplayedNumbersWithZPos = new List<(float, int)>();
    [SerializeField] [Tooltip("The angle at which the far-most torch can spawn")] [Range(0, 180)] private float Degrees = 180f;
    [SerializeField] private float DistanceOfTorchFromCenter = 5f;
    private List<int> EnteredOrder = new List<int>();
    private bool isPuzzleActive = false;
    [SerializeField] private Text3D NumberDisplay;
    [SerializeField] private int NumberOfTorches = 6;
    [SerializeField] private Objective objective;
    [SerializeField] private bool ShouldSortByZValue = false;
    private List<int> Soloution = new List<int>();
    private Dictionary<GameObject, Torch> TorchesInPuzzle = new Dictionary<GameObject, Torch>();
    [SerializeField] private GameObject TorchPrefab;
    public void LightAllTorches()
    {
        EnteredOrder.Clear();
        foreach (var torch in TorchesInPuzzle)
        {
            torch.Value.ToggleParticles(true);
        }
    }

    public void OnTorchExtinguish(int index)
    {
        EnteredOrder.Add(index);
        if (EnteredOrder[EnteredOrder.Count - 1] != Soloution[EnteredOrder.Count - 1])
        {
            LightAllTorches();
        }
        else
        {
            //Check if order is correct
            if (EnteredOrder.Count == Soloution.Count)
            {
                //OnCompletion
                objective.SetComplete();
            }
        }
    }

    //Creates a torch in a spherical radius around the player starting at x degrees to the left rotating round to right
    private void CreateTorches()
    {
        float DegreesBetweenTorch = Degrees * 2f / (NumberOfTorches);

        for (int i = 0; i < NumberOfTorches; i++)
        {
            float degreesOnCircle = Degrees - (i * DegreesBetweenTorch);
            float radians = degreesOnCircle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            Vector3 Position = transform.position + new Vector3(x * DistanceOfTorchFromCenter, 0, z * DistanceOfTorchFromCenter);
            GameObject Torch = Instantiate(TorchPrefab, Position, transform.rotation, transform);
            Torch.name = i.ToString();
            Torch component = Torch.GetComponent<Torch>();
            component.Puzzle = this;
            component.TorchIndex = i;
            TorchesInPuzzle.Add(Torch, component);
        }
    }

    //Sorts all numbers by there x position relative to the center of the puzzle
    private void FindOrder()
    {
        foreach (var Tupple in NumberDisplay.CurrentlyDisplayedNumber)
        {
            AllDisplayedNumbersWithZPos.Add((Tupple.Item1.transform.localPosition.z, Tupple.Item2));
        }

        AllDisplayedNumbersWithZPos = AllDisplayedNumbersWithZPos.OrderByDescending(x => x.Item1).ToList();

        foreach (var Tupple in AllDisplayedNumbersWithZPos)
        {
            Soloution.Add(Tupple.Item2);
        }
    }

    private void FixedUpdate()
    {
        if (isPuzzleActive && objective.IsActive())
        {
            foreach (var torch in TorchesInPuzzle)
            {
                torch.Value.ParentUpdate();
            }
        }
    }

    private void OnTriggerEnter()
    {
        isPuzzleActive = true;
    }

    private void OnTriggerExit()
    {
        isPuzzleActive = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateTorches();
        NumberDisplay.DisplayRandomUnique(NumberOfTorches);

        if (ShouldSortByZValue) FindOrder();
        else Soloution = NumberDisplay.GetSolution();
    }
}