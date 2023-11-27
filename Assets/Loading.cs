using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour
{
    float time = 0;
    [SerializeField] float TimeBetweenSwitch = .25f;

    int CurrentIndex = 0;
    [SerializeField] string[] Strings = new string[7]
    {
        "Loading","Loading.","Loading..","Loading...","Loading..","Loading.","Loading"
    };

    [SerializeField] TextMeshProUGUI textMeshProUgui;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > TimeBetweenSwitch)
        {
            time = 0;
            textMeshProUgui.text = Strings[CurrentIndex];

            CurrentIndex++;
            if (CurrentIndex >= Strings.Length)
            {
                CurrentIndex = 0;
            }
        }
    }
}
