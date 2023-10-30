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
    [SerializeField] bool ToggleState = false;
    [SerializeField] Animator Animator;
    public void Start()
    {

    }

    public void Update()
    {
        if (audio.clip != _idleClip && !audio.isPlaying && TorchActive) PlayIdleAudio();
        if (ToggleState)
        {
            ToggleState = false;
            ToggleParticles(!torchLight.enabled);
        };
    }

    public void ParentUpdate()
    {
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


    public void Light()
    {
        particleSystem.Play();

        audio.loop = false;
        audio.clip = _lightClip;
        audio.Play();

        TorchActive = true;
        torchLight.enabled = true;
    }

    public void Extinguish()
    {
        particleSystem.Stop();
        audio.Pause();
        audio.loop = false;
        audio.clip = _extinguishClip;
        audio.Play();

        TorchActive = false;
        torchLight.enabled = false;
    }

    public void ToggleParticles(bool ShouldBeLit)
    {
        Animator.SetBool("IsLit", ShouldBeLit);
    }
}