using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveUIAnim : BaseUIAnim
{
    [SerializeField, Tooltip("ディゾルブに使用するTexture")]
    private Texture2D _dissolveTex;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("ディゾルブの閾値")]
    private float _dissolveAmount;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("境目の太さ")]
    private float _dissolveRange;
    [SerializeField, ColorUsage(false, true), Tooltip("境目の色")]
    private Color _dissolveColor;
    [SerializeField, Tooltip("ディゾルブのシェーダー")]
    private Shader _shader;
    [SerializeField, Tooltip("DissolveTextureのTilling")]
    private Vector2 _dissolveTilling = new Vector2(1f, 1f);
    [SerializeField, Tooltip("DissolveTextureのOffset")]
    private Vector2 _dissolveOffset = new Vector2(0f, 0f);

    /// <summary>_DissolveTexのID</summary>
    private int _dissolveTexId = Shader.PropertyToID("_DissolveTex");
    /// <summary>_DissolveAmountのID</summary>
    private int _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    /// <summary>_DissolveRangeのID</summary>
    private int _dissolveRangeId = Shader.PropertyToID("_DissolveRange");
    /// <summary>_DissolveColorのID</summary>
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
