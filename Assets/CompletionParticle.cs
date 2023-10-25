using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CompletionParticle : MonoBehaviour
{
    [SerializeField] Animation RaiseClip;
    [SerializeField] Transform parentTransform;
    Material material;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] float MoveToTargetTime = 5f;

    public UnityEvent<Objective> OnComplete = new UnityEvent<Objective>();


    Vector3 TargetPos;
    Objective thisObjective;
    public void SetObjective(Objective set) => thisObjective = set;
    void Start()
    {

    }

    public void StartTransition(Vector3 TargetPos, Color ParticleColor)
    {
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = ParticleColor;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor",ParticleColor * 5f);
        renderer.material = material;

        this.TargetPos = TargetPos;
        RaiseClip.Play();
    }


    public void MoveTo()
    {
        parentTransform.DOMove(TargetPos, MoveToTargetTime, false).SetEase(Ease.InOutCirc).OnComplete(OnFinish);
    }

    void OnFinish()
    {
        OnComplete?.Invoke(thisObjective);
        Destroy(gameObject);
    }
}
