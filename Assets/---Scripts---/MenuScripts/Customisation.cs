using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{
    public Transform playerParent; // The parent GameObject where the player should be instantiated
    private GameObject[] playerPrefabs;
    private GameObject currentPlayer;
    private int currentPlayerIndex = 0;

    public event Action<int> OnPrefabSelected;

    public bool[] debugUnlockStatus;
    public List<PrefabSelector> prefabSelectors;


    void Start()
    {
        // Load all the player prefabs from the Resources/PlayerPrefabs folder into the array
        playerPrefabs = Resources.LoadAll<GameObject>("PlayerPrefabs");
        Debug.Log("Loaded prefabs: " + string.Join(", ", System.Array.ConvertAll(playerPrefabs, prefab => prefab.name)));
        // Instantiate the initial player prefab
        currentPlayer = Instantiate(playerPrefabs[currentPlayerIndex], playerParent.position, Quaternion.identity, playerParent);

        // Initialise unlock status for each orefab
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            if (!PlayerPrefs.HasKey("Prefab_" + i))
            {
                PlayerPrefs.SetInt("Prefab_" + i, i == 0 ? 1 : 0); // First prefab is unlocked by default
            }
        }

        debugUnlockStatus = new bool[playerPrefabs.Length];
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            debugUnlockStatus[i] = IsPrefabUnlocked(i);
        }
    }


    void Update()
    {
        for (int i = 0; i < prefabSelectors.Count; i++)
        {
            if (prefabSelectors[i] != null && prefabSelectors[i].gameObject.activeInHierarchy) // I think this was the issue
            {
                prefabSelectors[i].UpdateVisualRepresentation((IsPrefabUnlocked(i)));
            }
        }
    }


    public void NextPlayer()
    {
        Vector3 currentPosition = currentPlayer.transform.position;
        Transform currentTarget = currentPlayer.GetComponent<WaypointRunner>().GetCurrentTarget();
        bool isPaused = currentPlayer.GetComponent<WaypointRunner>().IsPaused();

        Destroy(currentPlayer);

        currentPlayerIndex = (currentPlayerIndex + 1) % playerPrefabs.Length;
        currentPlayer = Instantiate(playerPrefabs[currentPlayerIndex], playerParent.position, Quaternion.identity, playerParent);

        SetInitialState(currentPosition, currentTarget, isPaused);
    }

    public void PreviousPlayer()
    {
        Vector3 currentPosition = currentPlayer.transform.position;
        Transform currentTarget = currentPlayer.GetComponent<WaypointRunner>().GetCurrentTarget();
        bool isPaused = currentPlayer.GetComponent<WaypointRunner>().IsPaused();

        Destroy(currentPlayer);

        currentPlayerIndex--;
        if (currentPlayerIndex < 0)
        {
            currentPlayerIndex = playerPrefabs.Length - 1;
        }
        currentPlayer = Instantiate(playerPrefabs[currentPlayerIndex], playerParent.position, Quaternion.identity, playerParent);

        SetInitialState(currentPosition, currentTarget, isPaused);
    }


    public void SetInitialState(Vector3 position, Transform target, bool isPaused)
    {
        currentPlayer.transform.position = position;  // This line should set the new prefab's position
        WaypointRunner waypointRunner = currentPlayer.GetComponent<WaypointRunner>();
        waypointRunner.SetInitialState(target, isPaused);
    }

    public void SelectPrefab(int index)
    {
        Debug.Log("SelectPrefab called with index: " + index);
        // Store the selected prefab index for later use
        currentPlayerIndex = index;
        OnPrefabSelected?.Invoke(index);
    }


    public void ConfirmSelection()
    {
        Debug.Log("ConfirmSelection called. Current index: " + currentPlayerIndex);
        PlayerPrefs.SetInt("SelectedPrefab", currentPlayerIndex);
        PlayerPrefs.Save();

        Destroy(currentPlayer);
        currentPlayer = Instantiate(playerPrefabs[currentPlayerIndex], playerParent.position, Quaternion.identity, playerParent);
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public bool IsPrefabUnlocked(int index)
    {
        return PlayerPrefs.GetInt("Prefab_" + index, 0) == 1;
    }

    public void UnlockPrefab(int index)
    {
        PlayerPrefs.SetInt("Prefab_" + index, 1);
    }

    public void LockPrefab(int index)
    {
        PlayerPrefs.SetInt("Prefab_" + index, 0);
    }


    public void UpdateUnlockStatusFromDebugArray()
    {
        for (int i = 0; i < debugUnlockStatus.Length; i++)
        {
            if (debugUnlockStatus[i])
            {
                UnlockPrefab(i);
            }
            else
            {
                LockPrefab(i);
            }
        }
    }

    void OnValidate()
    {
        UpdateUnlockStatusFromDebugArray();
        UpdateVisualRepresentationOfPrefabs();
    }

    public void UpdateVisualRepresentationOfPrefabs()
    {
        for (int i = 0; i < debugUnlockStatus.Length; i++)
        {
            bool isUnlocked = IsPrefabUnlocked(i);
            prefabSelectors[i].UpdateVisualRepresentation(isUnlocked);
        }
    }



}