using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    #region Statics

    public static int NumberOfObjectives = 4;
    private static List<Objective> AllObjectivesInLevel { set; get; } = new List<Objective>();

    #endregion Statics

    #region Refrences

    [SerializeField] public static List<Objective> AllCurrentActiveObjectives = new List<Objective>();
    [SerializeField] private bool _debugCompleteAll = false;
    [SerializeField] private GameObject CompletionParticle;
    [SerializeField] private TaskDisplay taskDisplayer;
    [SerializeField] GameObject completionPoint;
    #endregion Refrences

    #region Events

    public static UnityEvent AllObjectivesComplete = new UnityEvent();
    public static UnityEvent<Objective> ObjectiveComplete = new UnityEvent<Objective>();
    #endregion Events

    //Called by each individual objective when it is created so its registered before we pick all tasks
    public static void AddObjective(Objective objectiveToAdd)
    {
        if (!AllObjectivesInLevel.Contains(objectiveToAdd)) AllObjectivesInLevel.Add(objectiveToAdd);
    }

    //Gets a random list of tasks defined within the count, that can only select a single task once
    public static List<Objective> GetRandomTasks(int count)
    {
        Objective[] allAvailableObjectivesArray = new Objective[AllObjectivesInLevel.Count];
        AllObjectivesInLevel.CopyTo(allAvailableObjectivesArray);
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
        completionPoint.SetActive(true);
    }

    //called when a single task has been completed via a public event
    private void OnObjectiveComplete(Objective completedObjective)
    {
        AllCurrentActiveObjectives.Remove(completedObjective);

        //Get All Relative data for the completion particle effect
        Vector3 ObjectivePosition = completedObjective.transform.position;
        Vector3 DisplayeePosition = TaskDisplay.Displayees[completedObjective].position;
        Color DisplayeeColor = TaskDisplay.Colors[completedObjective];

        //Spawn the particle and tell it where to go and what color it should be
        GameObject CompletionParticleInstance = Instantiate(CompletionParticle, ObjectivePosition, Quaternion.identity);
        CompletionParticle component = CompletionParticleInstance.GetComponentInChildren<CompletionParticle>();
        component.SetObjective(completedObjective);
        component.OnComplete?.AddListener(UpdateTaskDisplayOnCompletion);

        //Start the transition
        component.StartTransition(DisplayeePosition, DisplayeeColor);
    }

    private void Start()
    {
        AllCurrentActiveObjectives = GetRandomTasks(NumberOfObjectives);
        AllObjectivesInLevel = new List<Objective>();
        ObjectiveComplete.AddListener(OnObjectiveComplete);
    }

    private void Update()
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
    //This function is called once the completion particle has reached the intended target and causes the eyes to light up
    private void UpdateTaskDisplayOnCompletion(Objective completedObjective)
    {
        taskDisplayer.SetTaskDisplayComplete(completedObjective);
        if (CheckIfAllTasksCompleted()) OnAllTasksCompleted();
    }
}