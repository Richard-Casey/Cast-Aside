using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    [SerializeField] GameObject DisplayPrefab;
    [SerializeField] float ObjectWidth = 3f;
    [SerializeField] float ObjectSpacing = .5f;
    [SerializeField] float AvalibleSpace = 10f;
    [SerializeField] Gradient EyeColorGradient;

    void Start()
    {
        float Scale = AvalibleSpace / (ObjectiveManager.ObjectiveCount * (ObjectWidth + ObjectSpacing));

        ObjectSpacing *= Scale;
        ObjectWidth *= Scale;
        float TotalWidth = (ObjectWidth + ObjectSpacing) * ObjectiveManager.ObjectiveCount;


        float Radius = TotalWidth / 2f;

        for (int i = 0; i < ObjectiveManager.ObjectiveCount; i++)
        {
            Vector3 PosToSpawn = transform.position - new Vector3(Radius - ObjectWidth/2f, 0, 0);
            PosToSpawn.x += i * (ObjectSpacing + ObjectWidth);
            GameObject newObject = Instantiate(DisplayPrefab, PosToSpawn, Quaternion.identity);
            newObject.transform.localScale = new Vector3(Scale, Scale, Scale);
            newObject.transform.parent = transform;
            Light[] lights = newObject.GetComponentsInChildren<Light>();
            Color color = EyeColorGradient.Evaluate((float)i / (float)ObjectiveManager.ObjectiveCount);

            lights[0].color = color;
            lights[1].color = color;
        }
    }

}
