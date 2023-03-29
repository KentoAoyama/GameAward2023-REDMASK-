// 日本語対応
using System;
using UnityEngine;

public class PrepareCameraController2 : MonoBehaviour
{
    [SerializeField]
    private Control _testValue = default;
    [SerializeField]
    private Control _stage1 = default;
    [SerializeField]
    private Control _stage2 = default;
    [SerializeField]
    private Control _stage3 = default;
    [SerializeField]
    private Control _stage4 = default;

    private Control _useValue = default;

    private float _dir = 1;

    private void Start()
    {
        switch (GameManager.Instance.StageSelectManager.GoToStageType.Value)
        {
            case StageType.One:
                _useValue = _stage1;
                break;
            case StageType.Two:
                _useValue = _stage2;
                break;
            case StageType.Three:
                _useValue = _stage3;
                break;
            case StageType.Four:
                _useValue = _stage4;
                break;
            default:
                _useValue = _testValue;
                break;
        }
    }
    private void Update()
    {
        var pos = transform.position;
        pos.y += Time.deltaTime * _useValue.Speed * _dir;
        transform.position = pos;

        if (transform.position.y > _useValue.MaxValue && _dir > 0f)
        {
            pos = transform.position;
            pos.y = _useValue.MaxValue;
            transform.position = pos;

            _dir *= -1;
        }
        else if (transform.position.y < _useValue.MinValue && _dir < 0f)
        {
            pos = transform.position;
            pos.y = _useValue.MinValue;
            transform.position = pos;

            _dir *= -1;
        }
    }
    [Serializable]
    private class Control
    {
        [SerializeField]
        private float _speed = default;
        [SerializeField]
        private float _minValue = default;
        [SerializeField]
        private float _maxValue = default;

        public float Speed => _speed;
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
    }
}
