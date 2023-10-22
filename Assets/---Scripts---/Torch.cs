using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] Light torchLight;
    [SerializeField] ParticleSystem particleSystem;

    public void ToggleParticles()
    {
        torchLight.enabled = !torchLight.enabled;
        if(particleSystem.isStopped) particleSystem.Play();
        else particleSystem.Stop();
    }
}
