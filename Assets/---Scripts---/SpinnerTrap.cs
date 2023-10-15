using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerTrap : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Time.deltaTime * speed),
            transform.eulerAngles.z);
    }
}
