using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlurPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader _blurShader;
    [SerializeField]
    private PostProcessTiming _timing = PostProcessTiming.AfterOpaque;
    [SerializeField]
    private bool _applyToSceneView = true;

    private BlurPostProcessRenderPass _pass;

    public override void Create()
    {
        _pass = new BlurPostProcessRenderPass(_applyToSceneView, _blurShader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _pass.Setup(renderer.cameraColorTarget, _timing);
        renderer.EnqueuePass(_pass);
    }
}
