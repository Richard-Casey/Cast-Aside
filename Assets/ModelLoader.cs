using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLoader : MonoBehaviour
{

    GameObject[] playerPrefabs;
    [SerializeField] Animator animator;
    void Start()
    {
        // Load all the player prefabs from the Resources/PlayerPrefabs folder into the array
        playerPrefabs = Resources.LoadAll<GameObject>("PlayablePlayerPrefabs"); 
        // Instantiate the initial player prefab
        var go = Instantiate(playerPrefabs[PlayerPrefs.GetInt("SelectedPrefab")], transform.position, Quaternion.identity, transform);

        animator.runtimeAnimatorController = go.GetComponent<Animator>().runtimeAnimatorController;
        animator.avatar = go.GetComponent<Animator>().avatar;

    }
}
