using System.Collections.Generic;
using UnityEngine;

public class MaterialCheck : MonoBehaviour
{
    #region main

    void Start()
    {
    }

    private void OnDisable()
    {
        Shader.SetGlobalFloat("_Size", 0);
    }

    void OnApplicationQuit()
    {
        Shader.SetGlobalFloat("_Size", 0);
    }

    public LayerMask mask;
    public Camera camera;
    public CameraManager camManager;

    readonly List<MeshRenderer> RenderesActiveThisFrame = new();
    readonly List<MeshRenderer> AllActiveRenderers = new();

    public Material SeeThrough;
    public Material NonSeeThrough;

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


        var Distance = (transform.position - camera.transform.position).magnitude - 1;
        var Direction = -camera.transform.forward;
        var ray = new Ray(transform.position + Direction + Vector3.up, Direction);
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

    /*
    void OnDrawGizmos()
    {
        var Direction = (transform.position - camera.transform.position).normalized;

        Direction = camera.transform.rotation*Direction;
        var Distance = (transform.position - (camera.transform.position - camManager.GetOffset)).magnitude - 1;
        Vector3 Start = camera.transform.position - (Direction * -camera.nearClipPlane);
        Vector3 End = Start + Direction * Distance + (Direction * -camera.nearClipPlane);

        Gizmos.DrawSphere(Start, 1);
        Gizmos.DrawSphere(End, 1);


    }*/

    #endregion
}