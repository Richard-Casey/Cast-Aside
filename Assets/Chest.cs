using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Animation openAnimation;
    [SerializeField] GameObject point;
    public void Unlock(int id)
    {
        openAnimation.Play();
        point.SetActive(false);
        PlayerPrefs.SetInt("Prefab_" + id.ToString(), 1);
    }
}
