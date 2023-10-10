using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float SpinSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = transform.rotation.eulerAngles + new Vector3(0, Time.deltaTime + SpinSpeed, 0);
    }
}
