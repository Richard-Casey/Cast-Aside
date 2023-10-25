using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] bool TorchActive = true;
    [SerializeField] Light torchLight;
    [SerializeField] Transform TorchHead;
    [SerializeField] ParticleSystem particleSystem;
    public int TorchIndex;
    public TorchShadowPuzzleMaster Puzzle;
    float _timeinshadow = 0f;

    public void ToggleParticles(bool ShouldBeLit)
    {
        TorchActive = ShouldBeLit;
        torchLight.enabled = ShouldBeLit;
        if(ShouldBeLit && !particleSystem.isEmitting) particleSystem.Play();
        else if(!ShouldBeLit) particleSystem.Stop();
    }


    public void ParentUpdate()
    {
        if (!TorchActive) return;
        //Calculate the angle between the head of the torch and the shadow caster
        Vector3 SunDirection = Puzzle.sunTransform.forward;
        RaycastHit data;
        Debug.DrawRay(TorchHead.position, -SunDirection,Color.red,1f);
        if (Physics.Raycast(TorchHead.position, -SunDirection, out data,Puzzle.shadowCasterLayerMask))
        {
            if (data.transform.CompareTag("Sphere"))
            {
                _timeinshadow += Time.deltaTime;
                if(_timeinshadow >= Puzzle.TorchTimeNeededInShadowToGoOut)
                {
                    _timeinshadow = 0f;
                    ToggleParticles(false);
                    Puzzle.OnTorchExtinguish(TorchIndex);
                }
            }
            else
            {
                _timeinshadow = 0f;
            }
        }
    }
}
