using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] public static int ObjectiveCount = 4;
    [SerializeField] List<int> ActiveObjectIDs;

    public static UnityEvent<int> ObjectiveComplete = new UnityEvent<int>();
    public static UnityEvent AllObjectivesComplete = new UnityEvent();
    static Dictionary<int, Objective> ObjectivesInLevel { set; get; } = new Dictionary<int, Objective>();

    void Start()
    {
        ActiveObjectIDs = GetRandomTaskIDs(ObjectiveCount);
        ObjectiveComplete.AddListener(OnObjectiveComplete);
    }

    void OnObjectiveComplete(int id)
    {
        ActiveObjectIDs.Remove(id);
        if(CheckIfAllTasksCompleted())OnAllTasksCompleted();
    }

    bool CheckIfAllTasksCompleted()
    {
        if (ActiveObjectIDs.Count <= 0) return true;
        return false;
    }

    void OnAllTasksCompleted()
    {
        AllObjectivesComplete?.Invoke();
    }

    public static int? AddObjective(Objective objectiveToAdd)
    {
        if (ObjectivesInLevel.ContainsValue(objectiveToAdd)) return null;
        int id = ObjectivesInLevel.Count;
        ObjectivesInLevel.Add(id, objectiveToAdd);
        return id;
    }

    
    [CanBeNull]public static Objective GetObjectiveById(int id)
    {
        if (ObjectivesInLevel.TryGetValue(id, out Objective valueToReturn)) return valueToReturn;
        return null;
    }


    public static List<int> GetRandomTaskIDs(int count)
    {
        List<int> allAvalibleIds = ObjectivesInLevel.Keys.ToList();
        List<int> ReturnValues = new List<int>();

        count = Mathf.Min(count, allAvalibleIds.Count);

        for (int i = 0; i < count; i++)
        {
            int Rand = Random.Range(0, allAvalibleIds.Count - 1);
            ReturnValues.Add(allAvalibleIds[Rand]);
            allAvalibleIds.RemoveAt(Rand);
        }

        return ReturnValues;
    }
}
