using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePoint : MonoBehaviour
{
    [SerializeField] Transform maincamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = maincamera.forward;
    }
}
