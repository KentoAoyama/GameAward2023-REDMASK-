using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Costom Motion Blur")]
public class MotionBlurVolume : VolumeComponent
{
    [SerializeField, Tooltip("Žc‘œ‚Ì“§–¾“x")]
    private ClampedFloatParameter _alphaParameter = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

    public float AlphaParameter
    {
        get { return _alphaParameter.value; }
    }

    public bool IsActive() => _alphaParameter.value > 0.0f;
}
