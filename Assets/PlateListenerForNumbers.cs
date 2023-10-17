using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlateListenerForNumbers : MonoBehaviour
{
    [SerializeField] List<GameObject> PlatesForThis = new List<GameObject>();

    List <int> PlatePressOrder = new List<int>();

    [SerializeField] Text3D numberDisplay;
    [SerializeField] Objective ThisObjective;
    void Start()
    {
        foreach (var plate in PlatesForThis)
        {
            plate.GetComponent<PressurePlate>().OnPlateActivate?.AddListener(OnPlateDown);
        }
    }

    void OnDestroy()
    {
        foreach (var plate in PlatesForThis)
        {
            plate.GetComponent<PressurePlate>().OnPlateActivate?.RemoveListener(OnPlateDown);
        }
    }

    void OnPlateDown(GameObject plate)
    {
        PlatePressOrder.Add(PlatesForThis.IndexOf(plate));
        if (PlatePressOrder.Count == PlatesForThis.Count)
        {
            CheckOrder();
        }
    }

    void CheckOrder()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var number in PlatePressOrder)
        {
            sb.Append(number);
        }

        string insertedNumber = sb.ToString();

        if (insertedNumber == numberDisplay.NumberToDisplay)
        {
            Debug.Log("Correct");
            ThisObjective.SetComplete();
        }
        else
        {
            Debug.Log("false");
        }

        PlatePressOrder = new List<int>();
    }
}
