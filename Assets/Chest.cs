using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animation openAnimation;
    [SerializeField] GameObject point;
    [SerializeField] int id;

    public void Start()
    {
        //Disable Chest if already unlocked
        int? unlocked = null;

        unlocked = PlayerPrefs.GetInt("Prefab_" + id.ToString());
        if(unlocked == 1 || unlocked == null) gameObject.SetActive(false);

        //Disable if the previous unlockable hasnt been found yet
        if (id - 1 == 0) return;
        int? prevUnlocked = null;
        prevUnlocked = PlayerPrefs.GetInt("Prefab_" + (id - 1).ToString());

        if(prevUnlocked == 0) gameObject.SetActive(false);

    }

    public void Unlock()
    {
        openAnimation.Play();
        point.SetActive(false);
        PlayerPrefs.SetInt("Prefab_" + id.ToString(), 1);
    }
}
