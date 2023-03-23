// 日本語対応
using System;
using UnityEngine;

public class PrepareCameraController : MonoBehaviour
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
    private Camera _camera = default;

    private void Start()
    {
        _camera = GetComponent<Camera>();

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
        _camera.orthographicSize += Time.deltaTime * _useValue.Speed * _dir;

        if (_camera.orthographicSize > _useValue.MaxSize && _dir > 0f)
        {
            _camera.orthographicSize = _useValue.MaxSize;
            _dir *= -1;
        }
        else if (_camera.orthographicSize < _useValue.MinSize && _dir < 0f)
        {
            _camera.orthographicSize = _useValue.MinSize;
            _dir *= -1;
        }
    }
    [Serializable]
    private class Control
    {
        [SerializeField]
        private float _speed = default;
        [SerializeField]
        private float _minSize = default;
        [SerializeField]
        private float _maxSize = default;

        public float Speed => _speed;
        public float MinSize => _minSize;
        public float MaxSize => _maxSize;
    }
}
