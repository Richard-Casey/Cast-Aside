using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective : MonoBehaviour
{
    [SerializeField] string ObjectiveDescription = "Default Description";

    public bool IsComplete() => isComplete;
    public void SetComplete(){
        
        isComplete = true;
        ObjectiveManager.ObjectiveComplete?.Invoke(id);
    }
    public int GetId() => id;
    public void SetActive() => isActive = true;

    bool isComplete = false;
    bool isActive = false;
    [SerializeField] int id;

    void Start()
    {
        //Add To Global List of map Objectives
        int? idValue = ObjectiveManager.AddObjective(this);
        if (idValue.HasValue) id = idValue.Value;
    }
}
