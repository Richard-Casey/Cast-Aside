using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{
    public GameObject floatingHead;
    private MeshFilter meshFilter;

    private Mesh[] headMeshes;
    private int currentHeadIndex = 0;

    void Start()
    {
        meshFilter = floatingHead.GetComponent<MeshFilter>();

        // Load all the heads in the Resources/Heads folder into the array
        headMeshes = Resources.LoadAll<Mesh>("Heads");

        // Set the initial head mesh
        meshFilter.mesh = headMeshes[currentHeadIndex];
    }

    public void NextHead()
    {
        currentHeadIndex = (currentHeadIndex + 1) % headMeshes.Length;
        meshFilter.mesh = headMeshes[currentHeadIndex];
    }

    public void PreviousHead()
    {
        currentHeadIndex--;
        if (currentHeadIndex < 0)
        {
            currentHeadIndex = headMeshes.Length - 1;
        }

        meshFilter.mesh = headMeshes[currentHeadIndex];
    }
}
