using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMove : MonoBehaviour
{
    [SerializeField]
    private float _noiseStrength = 1.0F;
    [SerializeField]
    private float _noiseSpeed = 1.0F;

    private Vector3 _originalPosition = new Vector3();

    private void Awake()
    {
        _originalPosition = transform.localPosition;    
    }
    
    private void Update()
    {
        float noiseX = _noiseStrength * Mathf.PerlinNoise(Time.time * _noiseSpeed, 0.0F);
        float noiseY = _noiseStrength * Mathf.PerlinNoise(0.0F, Time.time * _noiseSpeed);

        transform.localPosition = new Vector3(_originalPosition.x + noiseX, _originalPosition.y + noiseY, _originalPosition.z);    
    }
}
