using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveUIAnim : BaseUIAnim
{
    [SerializeField, Tooltip("�f�B�]���u�Ɏg�p����Texture")]
    private Texture2D _dissolveTex;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("�f�B�]���u��臒l")]
    private float _dissolveAmount;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("���ڂ̑���")]
    private float _dissolveRange;
    [SerializeField, ColorUsage(false, true), Tooltip("���ڂ̐F")]
    private Color _dissolveColor;
    [SerializeField, Tooltip("�f�B�]���u�̃V�F�[�_�[")]
    private Shader _shader;
    [SerializeField, Tooltip("DissolveTexture��Tilling")]
    private Vector2 _dissolveTilling = new Vector2(1f, 1f);
    [SerializeField, Tooltip("DissolveTexture��Offset")]
    private Vector2 _dissolveOffset = new Vector2(0f, 0f);

    /// <summary>_DissolveTex��ID</summary>
    private int _dissolveTexId = Shader.PropertyToID("_DissolveTex");
    /// <summary>_DissolveAmount��ID</summary>
    private int _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    /// <summary>_DissolveRange��ID</summary>
    private int _dissolveRangeId = Shader.PropertyToID("_DissolveRange");
    /// <summary>_DissolveColor��ID</summary>
    private int _dissolveColorId = Shader.PropertyToID("_DissolveColor");


    protected override void UpdateMaterial(Material baseMatrial)
    {
        if (!_material)
        {
            _material = new Material(_shader);
            _material.CopyPropertiesFromMaterial(baseMatrial);
            _material.hideFlags = HideFlags.HideAndDontSave;
        }

        _material.SetTexture(_dissolveTexId, _dissolveTex);
        _material.SetTextureScale(_dissolveTexId, _dissolveTilling);
        _material.SetTextureOffset(_dissolveTexId, _dissolveOffset);
        _material.SetFloat(_dissolveAmountId, _dissolveAmount);
        _material.SetFloat(_dissolveRangeId, _dissolveRange);
        _material.SetColor(_dissolveColorId, _dissolveColor);
    }
}
