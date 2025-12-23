using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CelPostFeature : ScriptableRendererFeature
{
    class CelPostPass : ScriptableRenderPass
    {
        Material material;
        RenderTargetIdentifier source;
        RenderTargetHandle temp;

        public CelPostPass(Material mat)
        {
            material = mat;
            temp.Init("_CelPostTemp");
        }

        public void Setup(RenderTargetIdentifier src)
        {
            source = src;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Cel Post");

            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;

            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            cmd.GetTemporaryRT(temp.id, desc.width, desc.height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);

            cmd.SetGlobalTexture("_MainTex", source);
            cmd.Blit(source, temp.Identifier(), material);
            cmd.Blit(temp.Identifier(), source);

            cmd.ReleaseTemporaryRT(temp.id);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Shader shader;
    Material material;
    CelPostPass pass;

    public override void Create()
    {
        if (shader == null)
        {
            Debug.LogError("CelPostFeature shader is missing!");
            return;
        }

        material = CoreUtils.CreateEngineMaterial(shader);
        pass = new CelPostPass(material);
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

}
