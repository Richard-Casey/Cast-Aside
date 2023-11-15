using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITaskDisplay : MonoBehaviour
{

    [SerializeField] List<GameObject> ColumnParts = new List<GameObject>();
    [SerializeField] int CurrentPartIndex = 0;
    [SerializeField] float BlockHeight = 3f;
    

    void Start()
    {
        ObjectiveManager.ObjectiveComplete.AddListener(OnObjectiveComplete);
    }

    //.SetColor("_EmissionColor", TargetMaterials[(int)PushableBlocks[CellLocationOfPlayer].color].color * 2f))
    void OnObjectiveComplete(Objective completedObjective)
    {
        GameObject columPart = ColumnParts[CurrentPartIndex];

        MeshRenderer renderer = columPart.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_EmissionColor", TaskDisplay.Colors[completedObjective] * 10f);
        }

        columPart.SetActive(true);

        for (int i = 0; i <= CurrentPartIndex; i++)
        {
            ColumnParts[i].transform.DOLocalMoveY(BlockHeight + ColumnParts[i].transform.localPosition.y, 1);
        }

        CurrentPartIndex++;
    }


}
