using System.Collections.Generic;
using UnityEngine;

public class MaterialCheck : MonoBehaviour
{
    #region main

    void Start()
    {
    }

    public LayerMask mask;
    public Camera camera;
    public CameraManager camManager;

    readonly List<MeshRenderer> RenderesActiveThisFrame = new();
    readonly List<MeshRenderer> AllActiveRenderers = new();

    public float OpeningSize = 2f;
    public float MaxSize = 1f;
    float MinSize = 0f;
    float CurrentSize = 0f;



    void FixedUpdate()
    {
        /*Shader.SetGlobalVector("_GlobalPlayerPosition",transform.position + new Vector3(0,0,0));
        Shader.SetGlobalFloat("_Size", Size);
        Shader.SetGlobalFloat("_AngleThreshold", AngleThreshold);*/
        RenderesActiveThisFrame.Clear();

        Shader.SetGlobalFloat("_Size", CurrentSize);

        var Distance = (transform.position + Vector3.up - camera.transform.position).magnitude - 1;
        var Direction = -(camManager.GetOffset.normalized);
        var ray = new Ray(camera.transform.position - (Direction * -camera.nearClipPlane), Direction);
        RaycastHit[] Hits = Physics.SphereCastAll(ray.origin, .5f, ray.direction, Distance + (Direction * -camera.nearClipPlane).magnitude, mask);
        Debug.DrawRay(ray.origin,ray.direction,Color.red,2f);
        foreach (var data in Hits)
        {
            MeshRenderer renderer;
            Material material;
            if (data.transform.gameObject.TryGetComponent(out renderer))
            {
                if (renderer.material.shader.name == "Shader Graphs/SeeThroughCircle")
                {
                    if (!RenderesActiveThisFrame.Contains(renderer))
                    {
                        RenderesActiveThisFrame.Add(renderer);
                    }

                    if (!AllActiveRenderers.Contains(renderer))
                    {
                        AllActiveRenderers.Add(renderer);
                    }
                }
            }
        }



        if (RenderesActiveThisFrame.Count > 0)
        {
            CurrentSize = Mathf.MoveTowards(CurrentSize, MaxSize, Time.fixedDeltaTime);
        }
        else
        {
            CurrentSize = Mathf.MoveTowards(CurrentSize, MinSize, Time.fixedDeltaTime);
        }


    }

    void OnDrawGizmos()
    {
        var Direction = (transform.position - camera.transform.position).normalized;
        Debug.Log(Direction);
        var Distance = (transform.position + Vector3.up - camera.transform.position).magnitude - 1;
        Vector3 Start = camera.transform.position - (Direction * -camera.nearClipPlane);
        Vector3 End = Start + Direction * Distance + (Direction * -camera.nearClipPlane);

        Gizmos.DrawSphere(Start, 1);
        Gizmos.DrawSphere(End, 1);


    }

    #endregion
}