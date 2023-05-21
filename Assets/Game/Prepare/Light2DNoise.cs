using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light2DNoise : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _animCurve = new AnimationCurve();
    [SerializeField]
    private float _animLoopTime = 10F;

    private Light2D _light = default;
    private float _timer;
    private float _originalLightIntensity = 0.0F;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _originalLightIntensity = _light.intensity;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        _light.intensity = _originalLightIntensity * Mathf.Clamp01(_animCurve.Evaluate(_timer / _animLoopTime));

        if (_timer > _animLoopTime)
        {
            _timer -= _animLoopTime;
        }
    }
}
