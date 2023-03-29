using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class MonochromeController : MonoBehaviour
{
    [SerializeField, Tooltip("îíçïÇ…Ç»ÇËãÔçá"), Range(0.0f, 1.0f)]
    float _monoBlend;
    [SerializeField, Tooltip("VolumeÇÃIntencity"), Range(0.0f, 1.0f)]
    float _blurIntencity;
    [SerializeField]
    Volume _volume;

    private MotionBlur _motionBlur;

    public MotionBlur Blur
    {
        get 
        {
            if (_motionBlur == null)
            {
                _volume.profile.TryGet(out _motionBlur);
            }

            return _motionBlur; 
        }
    }
    /// <summary>_MonoBlendÇÃID</summary>
    int _monoBlendId = Shader.PropertyToID("_MonoBlend");
    private void Awake()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
        _volume.profile.TryGet(out _motionBlur);
        _motionBlur.intensity.value = _blurIntencity;
    }

    private void Update()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
        _motionBlur.intensity.value = _blurIntencity;
    }

    private void OnValidate()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
        Blur.intensity.value = _blurIntencity;
    }
}
