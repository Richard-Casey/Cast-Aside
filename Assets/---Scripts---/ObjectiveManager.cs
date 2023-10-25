using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    //The amount of tasks we wish to select this run
    public static int NumberOfObjectives = 4;

    #region Refrences 
    [SerializeField] bool _debugCompleteAll = false;
    [SerializeField] public static List<Objective> AllCurrentActiveObjectives;
    [SerializeField] TaskDisplay taskDisplayer;
    [SerializeField] GameObject CompletionParticle;
    #endregion
       
    #region Events
    public static UnityEvent<Objective> ObjectiveComplete = new UnityEvent<Objective>();
    public static UnityEvent AllObjectivesComplete = new UnityEvent();
    #endregion

    //Dict of all tasks in the level sorted by their IDs
    private static List<Objective> ObjectivesInLevel { set; get; } = new List<Objective>();

    private void Start()
    {
        AllCurrentActiveObjectives = GetRandomTasks(NumberOfObjectives);
        ObjectivesInLevel = new List<Objective>();

        ObjectiveComplete.AddListener(OnObjectiveComplete);
    }

    void Update()
    {
        if (_debugCompleteAll)
        {
            _debugCompleteAll = false;
            for (int i = AllCurrentActiveObjectives.Count - 1; i >= 0; i--)
            {
                AllCurrentActiveObjectives[i].SetComplete();
            }
        }
    }

    //called when a single task has been completed via a public event
    private void OnObjectiveComplete(Objective completedObjective)
    {
        AllCurrentActiveObjectives.Remove(completedObjective);

        Vector3 ObjectivePosition = completedObjective.transform.position;
        Vector3 DisplayeePosition = TaskDisplay.Displayees[completedObjective].position;
        Color DisplayeeColor = TaskDisplay.Colors[completedObjective];

        GameObject CompletionParticleInstance = Instantiate(CompletionParticle, ObjectivePosition, Quaternion.identity);
        CompletionParticle component = CompletionParticleInstance.GetComponentInChildren<CompletionParticle>();
        component.SetObjective(completedObjective);
        component.OnComplete?.AddListener(UpdateTaskDisplayOnCompletion);
        component.StartTransition(DisplayeePosition,DisplayeeColor);


    }

    private void UpdateTaskDisplayOnCompletion(Objective completedObjective)
    {
        taskDisplayer.SetTaskDisplayComplete(completedObjective);
        if (CheckIfAllTasksCompleted()) OnAllTasksCompleted();
    }

    //Check if all tasks have been completed
    private bool CheckIfAllTasksCompleted()
    {
        if (AllCurrentActiveObjectives.Count <= 0) return true;
        return false;
    }

    //Code here for when all tasks have been finished
    private void OnAllTasksCompleted()
    {
        AllObjectivesComplete?.Invoke();
    }

    //Called by each individual objective when it is created so its registered before we pick all tasks
    public static void AddObjective(Objective objectiveToAdd)
    {
        if(!ObjectivesInLevel.Contains(objectiveToAdd)) ObjectivesInLevel.Add(objectiveToAdd);
    }


    //Gets a random list of tasks defined within the count, that can only select a single task once
    public static List<Objective> GetRandomTasks(int count)
    {
        Objective[] allAvailableObjectivesArray = new Objective[ObjectivesInLevel.Count];
        ObjectivesInLevel.CopyTo(allAvailableObjectivesArray);
        List<Objective> allAvailableObjectives = allAvailableObjectivesArray.ToList();
        List<Objective> returnValues = new List<Objective>();

        count = Mathf.Min(count, allAvailableObjectives.Count);

        for (int i = 0; i < count; i++)
        {
            int Rand = Random.Range(0, allAvailableObjectives.Count - 1);
            returnValues.Add(allAvailableObjectives[Rand]);
            allAvailableObjectives[Rand].SetActive();
            allAvailableObjectives[Rand].SetId(i);
            allAvailableObjectives.RemoveAt(Rand);
        }

        return returnValues;
    }
}