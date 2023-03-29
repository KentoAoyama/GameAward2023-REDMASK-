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
    /// <summary>����Pass�̖��O</summary>
    private const string RenderPassName = nameof(MotionBlurRenderPass);
    /// <summary>�v���t�@�C���[�̖��O</summary>
    private const string ProfilingSamplerName = "SrcToDest";

    /// <summary>�|�X�g�G�t�F�N�g���V�[���r���[�ɂ����邩�ǂ���</summary>
    private readonly bool _applyToSceneView;
    /// <summary>�V�F�[�_�[���ϐ���_MainTex��ID</summary>
    private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");
    /// <summary>�|�X�g�G�t�F�N�g�������邽�߂̃}�e���A��</summary>
    private readonly Material _material;
    /// <summary>�}�e���A���ɕύX���������ނƂ��Ɏg�p����</summary>
    private readonly ProfilingSampler _profilingSampler;
    /// <summary>�V�F�[�_�[���ϐ���_Alpha��ID</summary>
    private readonly int _alphaPropertyId = Shader.PropertyToID("_Alpha");

    /// <summary>�|�X�g�G�t�F�N�g��������ꂽ�J���[�e�N�X�`��</summary>
    private RenderTargetHandle _afterPostProcessTexture;
    /// <summary>�J��������̃e�N�X�`��</summary>
    private RenderTargetIdentifier _cameraColorTarget;
    /// <summary>�ꎞ�I�ȃe�N�X�`��</summary>
    private RenderTargetHandle _tempRenderTargetHandle;
    /// <summary>�{�����[��</summary>
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

        // AfterRendering�̏ꍇ����Ō��ʂ��������e�N�X�`�����擾�ł���
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

        // �J�����̃|�X�g�v���Z�X�̐ݒ�
        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        // �V�[���r���[�֌W
        if (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView)
        {
            return;
        }

        // �{�����[�����L�����ǂ���
        if (!_volume.IsActive())
        {
            return;
        }

        var source = _cameraColorTarget;

        //AfterRendering�̏ꍇ�A_afterPostProcessTexture���g��
        if (renderPassEvent == RenderPassEvent.AfterRendering && renderingData.cameraData.resolveFinalTarget)
        {
            source = _afterPostProcessTexture.Identifier();
        }

        // �R�}���h�o�b�t�@���쐬
        var cmd = CommandBufferPool.Get(RenderPassName);
        cmd.Clear();

        // �J�����Ɠ���RenderTexture���擾����
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
