using UnityEngine;

public class Torch : MonoBehaviour
{
    public TorchShadowPuzzleMaster Puzzle;
    public int TorchIndex;
    private float _timeinshadow = 0f;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private bool TorchActive = true;
    [SerializeField] private Transform TorchHead;
    [SerializeField] private Light torchLight;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip _lightClip;
    [SerializeField] AudioClip _idleClip;
    [SerializeField] AudioClip _extinguishClip;

    public void Start()
    {

    }

    public void ParentUpdate()
    {
        if(audio.clip != _idleClip && !audio.isPlaying) PlayIdleAudio();

        if (!TorchActive) return;
        //Calculate the angle between the head of the torch and the shadow caster
        Vector3 SunDirection = Puzzle.sunTransform.forward;
        RaycastHit data;
        Debug.DrawRay(TorchHead.position, -SunDirection, Color.red, 1f);
        if (Physics.Raycast(TorchHead.position, -SunDirection, out data, Puzzle.shadowCasterLayerMask))
        {
            if (data.transform.CompareTag("Sphere"))
            {
                _timeinshadow += Time.deltaTime;
                if (_timeinshadow >= Puzzle.TorchTimeNeededInShadowToGoOut)
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

    void PlayIdleAudio()
    {
        audio.loop = true;
        audio.clip = _idleClip;
        audio.Play();
    }

    public void ToggleParticles(bool ShouldBeLit)
    {
        if (torchLight.enabled != ShouldBeLit)
        {
            audio.loop = false;
            if (ShouldBeLit) audio.clip = _lightClip;
            else audio.clip = _extinguishClip;
            audio.Play();
        }

        TorchActive = ShouldBeLit;
        torchLight.enabled = ShouldBeLit;
        if (ShouldBeLit && !particleSystem.isEmitting) particleSystem.Play();
        else if (!ShouldBeLit) particleSystem.Stop();
    }
}