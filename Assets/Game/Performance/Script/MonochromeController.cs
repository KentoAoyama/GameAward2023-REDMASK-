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
    
    /// <summary>_MonoBlendÇÃID</summary>
    int _monoBlendId = Shader.PropertyToID("_MonoBlend");
    private void Awake()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
    }

    private void Update()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
    }

    private void OnValidate()
    {
        Shader.SetGlobalFloat(_monoBlendId, _monoBlend);
    }
}
