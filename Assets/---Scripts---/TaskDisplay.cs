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
        //Calculate the size based on the avalible space and the amount of totems we need to spawn
        float Scale = AvalibleSpace / (ObjectiveManager.ObjectiveCount * (ObjectWidth + ObjectSpacing));

        //Apply this scale to the spacing and width to scaley them relative to the size of the totem
        ObjectSpacing *= Scale;
        ObjectWidth *= Scale;

        //Calculate the total area needed to spawn the objects
        float TotalWidth = (ObjectWidth + ObjectSpacing) * ObjectiveManager.ObjectiveCount;

        //Find half the total width so we can spawn left to right
        float Radius = TotalWidth / 2f;

        for (int i = 0; i < ObjectiveManager.ObjectiveCount; i++)
        {
            //Calculate the spawn position based of its index of spawning and width of object
            Vector3 PosToSpawn = transform.position - new Vector3(Radius - ObjectWidth/2f, 0, 0);
            PosToSpawn.x += i * (ObjectSpacing + ObjectWidth);

            //Create the object, scale, parent and change the color of its eyes
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
