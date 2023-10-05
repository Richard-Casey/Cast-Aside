using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizeFeature : ScriptableRendererFeature
{
    //Settings for the custom pass
    [SerializeField] CustomPassSettings settings;
    private pixelizePass customPass;

    public override void Create()
    {
        customPass = new pixelizePass(settings);

    }

    [System.Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 100;
        public LayerMask layerMask = -1;
        public Color backgroundColor = Color.black;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Prevent pass rendering in scene view camera
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(customPass);
        //renderer.EnqueuePass(defaultPass);

    }
}


public class pixelizePass : ScriptableRenderPass
{
    PixelizeFeature.CustomPassSettings settings;

    RenderTargetIdentifier colorBuffer, pixelBuffer , pixelBuffer2;
    int pixelBufferID = Shader.PropertyToID("_PixelBuffer");


    Material pixelMaterial;
    Material defaultMaterial;

    int pixelScreenHeight, pixelScreenWidth;


    //Layer Filtering
    LayerMask pixelLayerMask;
    LayerMask defaLayerMask;


    List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

    public pixelizePass(PixelizeFeature.CustomPassSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        this.pixelLayerMask = settings.layerMask;

        //drawing settings default values ??? copied from default "Render objects" feature
        m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
        m_ShaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
        

        if (pixelMaterial  == null) pixelMaterial = CoreUtils.CreateEngineMaterial("Custom/Pixelize");
    }


    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        



        colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        pixelScreenHeight = settings.screenHeight;
        pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

        pixelMaterial.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        pixelMaterial.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        pixelMaterial.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

        descriptor.height = pixelScreenHeight;
        descriptor.width = pixelScreenWidth;

        cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
        pixelBuffer = new RenderTargetIdentifier(pixelBufferID);

    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {

        if (!pixelMaterial) return;
        Camera camera = renderingData.cameraData.camera;

        SortingCriteria sortingCriteria = SortingCriteria.SortingLayer;
        
        
        DrawingSettings pixelDrawingSettings =
            CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);


        FilteringSettings pixelFilter = new FilteringSettings(RenderQueueRange.all, pixelLayerMask);
        


        CommandBuffer cmd = CommandBufferPool.Get();
        
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();


            renderingData.cameraData.camera.depthTextureMode = DepthTextureMode.Depth;
            
            cmd.SetRenderTarget(renderingData.cameraData.renderer.cameraColorTarget);
            //Draw Our Pixelated Content To The Camera
            context.DrawRenderers(renderingData.cullResults, ref pixelDrawingSettings, ref pixelFilter);

            //Copy our screen to a temp buffer and apply the material in the process
            cmd.Blit(colorBuffer, pixelBuffer);

            //Copy back so we have the material applied
            cmd.Blit(pixelBuffer,colorBuffer, pixelMaterial);


            context.ExecuteCommandBuffer(cmd);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);

    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(pixelBufferID);
    }
}
