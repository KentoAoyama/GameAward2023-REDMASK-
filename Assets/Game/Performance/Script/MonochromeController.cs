using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class MonochromeController : MonoBehaviour
{
    private FloatReactiveProperty _monoblend = new FloatReactiveProperty();
    
    /// <summary>_MonoBlend‚ÌID</summary>
    int _monoBlendId = Shader.PropertyToID("_MonoBlend");
    private void Awake()
    {
        _monoblend.Subscribe(x => Shader.SetGlobalFloat(_monoBlendId, x));
    }

    public void SetMonoBlend(float endValue, float duration = 1.0f)
    {
        endValue = Mathf.Clamp01(endValue);
        DOTween.To(() => _monoblend.Value, x => _monoblend.Value = x, endValue, duration);
    }
}
