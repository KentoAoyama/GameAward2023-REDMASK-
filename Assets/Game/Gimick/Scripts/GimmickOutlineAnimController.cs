using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickOutlineAnimController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve = default;
    [SerializeField]
    private float _loopSeconds = 1.0F;
    [SerializeField]
    private Shader _outlineShader = default;
    [SerializeField]
    private float _outlineRange = 5.0F;
    [SerializeField, ColorUsage(true, true)]
    private Color _outlineColor = Color.red;

    private SpriteRenderer _renderer;
    private float _timer = 0.0F;
    private int _outlineRangeId = Shader.PropertyToID("_OutlineRange");
    private int _outlineColorId = Shader.PropertyToID("_OutlineColor");
    private bool _wasPlayed = false;
    public bool WasPlayed
    {
        get => _wasPlayed;
        set => _wasPlayed = value;
    }
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.material = new Material(_outlineShader);
    }

    private void Update()
    {
        if (!_wasPlayed)
        {
            _timer += Time.deltaTime;

            _renderer.material.SetFloat(_outlineRangeId, Mathf.Clamp01(_curve.Evaluate(_timer)) * _outlineRange);
            _renderer.material.SetColor(_outlineColorId, _outlineColor);

            if (_loopSeconds < _timer)
            {
                _timer -= _loopSeconds; 
            }
        }
        else
        {
            _renderer.material.SetFloat(_outlineRangeId, 0.0F);
        }
    }
}
