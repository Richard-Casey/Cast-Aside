using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITaskDisplay : MonoBehaviour
{
    [SerializeField] GameObject TaskDisplayPrefab;
    [SerializeField] Transform UICamera;
    [SerializeField] Transform TaskDisplayPos;

    [SerializeField] private float ObjectWidth = 3f;
    [SerializeField] private float ObjectSpacing = .5f;
    [SerializeField] float scale = .2f;
  

    void Start()
    {

        int index = 0;
        foreach (var objective in TaskDisplay.Colors)
        {
            Vector3 PosToSpawn  = Vector3.zero;
            PosToSpawn.x -= (TaskDisplay.Colors.Count / 2f);
            PosToSpawn.x += index * (ObjectWidth + ObjectSpacing);

            GameObject display = GameObject.Instantiate(TaskDisplayPrefab,TaskDisplayPos);
            display.transform.localPosition = PosToSpawn;
            display.transform.rotation = Quaternion.LookRotation(-transform.forward,Vector3.up);
            display.layer = LayerMask.NameToLayer("UI");
            display.transform.localScale = new Vector3(scale, scale, scale);

            Light[] lightsInChild = display.GetComponentsInChildren<Light>();

            lightsInChild[0].color = objective.Value;
            lightsInChild[1].color = objective.Value;

            lightsInChild[0].enabled = false;
            lightsInChild[1].enabled = false;
            index++; 
        }
    }


}
