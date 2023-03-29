using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum PostprocessTiming
{
    AfterOpaque,
    BeforePostprocess,
    AfterPostprocess
}

public class MotionBlurRenderPass : ScriptableRenderPass
{
    /// <summary>このPassの名前</summary>
    private const string RenderPassName = nameof(MotionBlurRenderPass);
    /// <summary>プロファイラーの名前</summary>
    private const string ProfilingSamplerName = "SrcToDest";

    /// <summary>ポストエフェクトをシーンビューにかけるかどうか</summary>
    private readonly bool _applyToSceneView;
    /// <summary>シェーダー内変数の_MainTexのID</summary>
    private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
    /// <summary>ポストエフェクトをかけるためのマテリアル</summary>
    private readonly Material _material;
    /// <summary>マテリアルに変更を書き込むときに使用する</summary>
    private readonly ProfilingSampler _profilingSampler;
    /// <summary>シェーダー内変数の_AlphaのID</summary>
    private readonly int _alphaPropertyId = Shader.PropertyToID("_Alpha");

    /// <summary>ポストエフェクトがかけられたカラーテクスチャ</summary>
    private RenderTargetHandle _afterPostProcessTexture;
    /// <summary>カメラからのテクスチャ</summary>
    private RenderTargetIdentifier _cameraColorTarget;
    /// <summary>一時的なテクスチャ</summary>
    private RenderTargetHandle _tempRenderTargetHandle;
    /// <summary>ボリューム</summary>
    private MotionBlurVolume _volume;

    public MotionBlurRenderPass(bool applyToSceneView, Shader shader)
    {
        if (!shader)
        {
            return;
        }

        _applyToSceneView = applyToSceneView;
        _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
        _tempRenderTargetHandle.Init("_TempRT");

        _material = CoreUtils.CreateEngineMaterial(shader);

        // AfterRenderingの場合これで効果をかけたテクスチャを取得できる
        _afterPostProcessTexture.Init("_AfterPostProcessTexture");
    }
    
    public void Setup (RenderTargetIdentifier cameraColorTarget, PostprocessTiming timing)
    {
        _cameraColorTarget = cameraColorTarget;

        renderPassEvent = GetRenderPassEvent(timing);

        var volumeStack = VolumeManager.instance.stack;
        _volume = volumeStack.GetComponent<MotionBlurVolume>();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!_material)
        {
            return;
        }

        // カメラのポストプロセスの設定
        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        // シーンビュー関係
        if (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView)
        {
            return;
        }

        // ボリュームが有効かどうか
        if (!_volume.IsActive())
        {
            return;
        }

        var source = _cameraColorTarget;

        //AfterRenderingの場合、_afterPostProcessTextureを使う
        if (renderPassEvent == RenderPassEvent.AfterRendering && renderingData.cameraData.resolveFinalTarget)
        {
            source = _afterPostProcessTexture.Identifier();
        }

        // コマンドバッファを作成
        var cmd = CommandBufferPool.Get(RenderPassName);
        cmd.Clear();

        // カメラと同じRenderTextureを取得する
        var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        tempTargetDescriptor.depthBufferBits = 0;
        cmd.GetTemporaryRT(_tempRenderTargetHandle.id, tempTargetDescriptor);

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            _material.SetFloat(_alphaPropertyId, _volume.AlphaParameter);
            cmd.SetGlobalTexture(_mainTexPropertyId, source);

            Blit(cmd, source, _tempRenderTargetHandle.Identifier(), _material);
        }

        Blit(cmd, _tempRenderTargetHandle.Identifier(), source);

        //cmd.ReleaseTemporaryRT(_tempRenderTargetHandle.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    private static RenderPassEvent GetRenderPassEvent(PostprocessTiming timing)
    {
        switch (timing)
        {
            case PostprocessTiming.AfterOpaque :
                return RenderPassEvent.AfterRenderingSkybox;
            case PostprocessTiming.BeforePostprocess :
                return RenderPassEvent.BeforeRenderingPostProcessing;
            case PostprocessTiming.AfterPostprocess :
                return RenderPassEvent.AfterRendering;
            default:
                throw new ArgumentOutOfRangeException(nameof(PostprocessTiming), timing, null);
        }
    }
}
