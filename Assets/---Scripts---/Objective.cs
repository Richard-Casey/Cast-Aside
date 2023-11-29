using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [DisallowMultipleComponent]
public class Objective : MonoBehaviour
{
    [SerializeField] string ObjectiveDescription = "Default Description";
    [SerializeField] GameObject QuestMapMarker;
    public bool IsComplete() => isComplete;
    public void SetComplete()
    {

        if (!isActive) return;
        QuestMapMarker.SetActive(false);
        isActive = false;
        isComplete = true;
        ObjectiveManager.ObjectiveComplete?.Invoke(this);
    }
    public int GetId() => id;
    public int SetId(int id) => this.id = id;
    public void SetActive() => isActive = true;

    public bool IsActive() => isActive;

    bool isComplete = false;
    [SerializeField] bool isActive = false;
    [SerializeField] int id;
    [SerializeField] bool _forceSetComplete = false;

    void Start()
    {
        //Add To Global List of map Objectives
        ObjectiveManager.AddObjective(this);
    }


    void Update()
    {
        if (_forceSetComplete)
        {
            SetComplete();
            _forceSetComplete = false;
        }
    }
}
