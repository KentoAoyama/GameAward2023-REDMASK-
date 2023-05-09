using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable, VolumeComponentMenu("Post-processing/Blur")]
public class BlurPostProcessVolume : VolumeComponent
{
    public bool IsActive() => _blurStrength.value > 0;

    [SerializeField]
    private FloatParameter _blurStrength = new FloatParameter(0f);

    public FloatParameter BlurStrength => _blurStrength;
}
