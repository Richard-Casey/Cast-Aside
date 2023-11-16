using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public void Unlock(int id)
    {
        PlayerPrefs.SetInt("Prefab_" + id.ToString(), 1);
    }
}
