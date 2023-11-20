using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IDamagable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    float MaxHealth = 10f;
    float Health = 10f;
    float Resistance;
    float Worth;
}
