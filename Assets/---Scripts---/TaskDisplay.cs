using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private GameObject DisplayPrefab;
    [SerializeField] private float ObjectWidth = 3f;
    [SerializeField] private float ObjectSpacing = .5f;
    [SerializeField] private float AvalibleSpace = 10f;
    [SerializeField] private Gradient EyeColorGradient;

    public static Dictionary<Objective, Transform> Displayees = new Dictionary<Objective, Transform>();
    public static Dictionary<Objective, Color> Colors = new Dictionary<Objective, Color>();

    private void Start()
    {
        //Calculate the size based on the avalible space and the amount of totems we need to spawn
        float Scale = AvalibleSpace / (ObjectiveManager.NumberOfObjectives * (ObjectWidth + ObjectSpacing));

        //Apply this scale to the spacing and width to scaley them relative to the size of the totem
        ObjectSpacing *= Scale;
        ObjectWidth *= Scale;

        //Calculate the total area needed to spawn the objects
        float TotalWidth = (ObjectWidth + ObjectSpacing) * ObjectiveManager.NumberOfObjectives;

        //Find half the total width so we can spawn left to right
        float Radius = TotalWidth / 2f;

        for (int i = 0; i < ObjectiveManager.AllCurrentActiveObjectives.Count; i++)
        {
            //Calculate the spawn position based of its index of spawning and width of object
            Vector3 PosToSpawn = transform.position - new Vector3(Radius - ObjectWidth / 2f, 0, 0);
            PosToSpawn.x += i * (ObjectSpacing + ObjectWidth);

            //Move the point from local to world
            PosToSpawn = MathUtil.RotateAroundPivot(PosToSpawn, transform.position, transform.rotation);

            //Create the object, scale, parent and change the color of its eyes
            GameObject newObject = Instantiate(DisplayPrefab, PosToSpawn, transform.rotation);

            newObject.transform.localScale = new Vector3(Scale, Scale, Scale);
            newObject.transform.parent = transform;
            Light[] lights = newObject.GetComponentsInChildren<Light>();
            Color color = EyeColorGradient.Evaluate((float)i / (float)ObjectiveManager.NumberOfObjectives);
            lights[0].color = color;
            lights[1].color = color;
            lights[0].enabled = false;
            lights[1].enabled = false;

            Displayees.Add(ObjectiveManager.AllCurrentActiveObjectives[i], newObject.transform);
            Colors.Add(ObjectiveManager.AllCurrentActiveObjectives[i], color);
        }
    }

    public void SetTaskDisplayComplete(Objective completedObjective)
    {
        Light[] lights = Displayees[completedObjective].GetComponentsInChildren<Light>();
        lights[0].enabled = true;
        lights[1].enabled = true;
    }
}