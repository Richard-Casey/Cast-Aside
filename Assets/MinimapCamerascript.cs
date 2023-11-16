using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MinimapCamerascript : MonoBehaviour
{
    [SerializeField] Camera thisCamera;
    [SerializeField] bool doWeHaveFogInScene;

    private void Start()
    {
        doWeHaveFogInScene = RenderSettings.fog;

        RenderPipelineManager.beginCameraRendering += PreRender;
        RenderPipelineManager.endCameraRendering += PostRender;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= PreRender;
        RenderPipelineManager.endCameraRendering -= PreRender;
    }


    private void PreRender(ScriptableRenderContext content, Camera camera)
    {
        if (camera == thisCamera)
        {
            RenderSettings.fog = false;
        }
    }
    private void PostRender(ScriptableRenderContext content, Camera camera)
    {
        
        RenderSettings.fog = true;
        
    }
}
