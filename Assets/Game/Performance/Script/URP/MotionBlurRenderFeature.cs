using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class MotionBlurRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader _shader;
    [SerializeField]
    private PostprocessTiming _timing = PostprocessTiming.AfterOpaque;
    [SerializeField]
    private bool _applyToSceneView;

    private MotionBlurRenderPass _renderPass;

    public override void Create()
    {
        _renderPass = new(_applyToSceneView, _shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _renderPass.Setup(renderer.cameraColorTarget, _timing);
        renderer.EnqueuePass(_renderPass);
    }
}
