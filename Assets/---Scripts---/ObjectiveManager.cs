using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    //The amount of tasks we wish to select this run
    public static int ObjectiveCount = 4;

    [SerializeField] List<int> _activeObjectIDs;
    [SerializeField] TaskDisplay taskDisplayer;

    //Unity events
    public static UnityEvent<int> ObjectiveComplete = new UnityEvent<int>();

    public static UnityEvent AllObjectivesComplete = new UnityEvent();

    //Dict of all tasks in the level sorted by their IDs
    private static Dictionary<int, Objective> ObjectivesInLevel { set; get; } = new Dictionary<int, Objective>();
    int[] AllTasks;
    private void Start()
    {
        _activeObjectIDs = GetRandomTaskIDs(ObjectiveCount);
        AllTasks = new int[ObjectiveManager.ObjectiveCount];
        _activeObjectIDs.CopyTo(AllTasks);
        ObjectiveComplete.AddListener(OnObjectiveComplete);
    }

    //called when a single task has been completed via a public event
    private void OnObjectiveComplete(int id)
    {
        _activeObjectIDs.Remove(id);
        taskDisplayer.SetTaskDisplayComplete(AllTasks.ToList().IndexOf(id));
        if (CheckIfAllTasksCompleted()) OnAllTasksCompleted();
    }

    //Check if all tasks have been completed
    private bool CheckIfAllTasksCompleted()
    {
        if (_activeObjectIDs.Count <= 0) return true;
        return false;
    }

    //Code here for when all tasks have been finished
    private void OnAllTasksCompleted()
    {
        AllObjectivesComplete?.Invoke();
    }

    //Called by each individual objective when it is created so its registered before we pick all tasks
    public static int? AddObjective(Objective objectiveToAdd)
    {
        if (ObjectivesInLevel.ContainsValue(objectiveToAdd)) return null;
        int id = ObjectivesInLevel.Count;
        ObjectivesInLevel.Add(id, objectiveToAdd);
        return id;
    }

    //Gets the task by its associated id
    [CanBeNull]
    public static Objective GetObjectiveById(int id)
    {
        if (ObjectivesInLevel.TryGetValue(id, out Objective valueToReturn)) return valueToReturn;
        return null;
    }

    //Gets a random list of tasks defined within the count, that can only select a single task once
    public static List<int> GetRandomTaskIDs(int count)
    {
        List<int> allAvailableIds = ObjectivesInLevel.Keys.ToList();
        List<int> returnValues = new List<int>();

        count = Mathf.Min(count, allAvailableIds.Count);

        for (int i = 0; i < count; i++)
        {
            int Rand = Random.Range(0, allAvailableIds.Count - 1);
            returnValues.Add(allAvailableIds[Rand]);
            allAvailableIds.RemoveAt(Rand);
        }

        return returnValues;
    }
}