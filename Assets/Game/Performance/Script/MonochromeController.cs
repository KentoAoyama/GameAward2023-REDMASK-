using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MonochromeController
{
    private FloatReactiveProperty _monoblend = new FloatReactiveProperty(0.0f);
    
    /// <summary>_MonoBlendのID</summary>
    int _monoBlendId = Shader.PropertyToID("_MonoBlend");
    public MonochromeController()
    {
        _monoblend.Subscribe(x => Shader.SetGlobalFloat(_monoBlendId, x));
    }

    /// <summary>Monochromeにするための閾値を変更する</summary>
    /// <param name="endValue">0～1のあたいでどの程度白黒にするか</param>
    /// <param name="duration">何秒かけて変化させるか デフォルトは1</param>
    public void SetMonoBlend(float endValue, float duration = 1.0f)
    {
        endValue = Mathf.Clamp01(endValue);
        DOTween.To(() => _monoblend.Value, x => _monoblend.Value = x, endValue, duration);
    }
}
