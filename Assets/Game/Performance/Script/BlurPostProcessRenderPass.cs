using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum PostProcessTiming
{
    AfterOpaque,
    BeforePostProcess,
    AfterPostProcess
}

public class BlurPostProcessRenderPass : ScriptableRenderPass
{
    private const string RenderPassName = nameof(BlurPostProcessRenderPass);
    private const string ProfilingSamplerName = "SrcToDest";

    private readonly bool _applyToSceneView;
    private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
    private readonly Material _material;
    private readonly ProfilingSampler _profilingSampler;
    private readonly int _blurStrengthPropertyId = Shader.PropertyToID("_BlurStrength");

    /// <summary>ポストプロセスがかけられたテクスチャ</summary>
    private RenderTargetHandle _afterPostProcessTexture;
    /// <summary>カメラのレンダリングターゲット</summary>
    private RenderTargetIdentifier _cameraColorTarget;
    /// <summary>一時的なテクスチャ</summary>
    private RenderTargetHandle _tempRenderTargetHandle;
    private BlurPostProcessVolume _volume;

    public BlurPostProcessRenderPass(bool applyToSceneView, Shader shader)
    {
        if (!shader)
        {
            return;
        }

        _applyToSceneView = applyToSceneView;
        _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
        _tempRenderTargetHandle.Init("_TempRT");

        _material = CoreUtils.CreateEngineMaterial(shader);

        _afterPostProcessTexture.Init("_AfterPostProcessRT");
    }

    public void Setup(RenderTargetIdentifier cameraColorTarget, PostProcessTiming timing)
    {
        _cameraColorTarget = cameraColorTarget;
        
        renderPassEvent = GetRenderPassEvent(timing);

        var VolumeStack = VolumeManager.instance.stack;
        _volume = VolumeStack.GetComponent<BlurPostProcessVolume>();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!_material || !renderingData.cameraData.postProcessEnabled ||
            (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView) ||
            !_volume.IsActive())
        {
            return;
        }

        var source = renderPassEvent == RenderPassEvent.AfterRendering && renderingData.cameraData.resolveFinalTarget
           ? _afterPostProcessTexture.Identifier()
           : _cameraColorTarget;

        var cmd = CommandBufferPool.Get(RenderPassName);
        cmd.Clear();

        var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        tempTargetDescriptor.depthBufferBits = 0;
        cmd.GetTemporaryRT(_tempRenderTargetHandle.id, tempTargetDescriptor);

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            _material.SetFloat(_blurStrengthPropertyId, _volume.BlurStrength.value);
            cmd.SetGlobalTexture(_mainTexPropertyId, source);

            Blit(cmd, source, _tempRenderTargetHandle.Identifier(), _material);
        }

        Blit(cmd, _tempRenderTargetHandle.Identifier(), source);

        cmd.ReleaseTemporaryRT(_tempRenderTargetHandle.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    private RenderPassEvent GetRenderPassEvent(PostProcessTiming postprocessTiming)
    {
        switch (postprocessTiming)
        {
            case PostProcessTiming.AfterOpaque:
                return RenderPassEvent.AfterRenderingSkybox;
            case PostProcessTiming.BeforePostProcess:
                return RenderPassEvent.BeforeRenderingPostProcessing;
            case PostProcessTiming.AfterPostProcess:
                return RenderPassEvent.AfterRendering;
            default:
                throw new ArgumentOutOfRangeException(nameof(postprocessTiming), postprocessTiming, null);
        }
    }
}
