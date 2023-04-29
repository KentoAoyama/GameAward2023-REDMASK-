// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class PerformanceEvent
{
    [SerializeField]
    private bool _isWaitFireInput = default;
    [SerializeField]
    private float _time = default;
    [SerializeField]
    private UnityEvent _awake = default;
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private string _messageText = default;
    [SerializeField]
    private PerformanceMovement[] _move = default;
    [SerializeField]
    private PerformanceAnimation[] _animation = default;

    private List<TweenerCore<Vector3, Vector3, VectorOptions>> _mover = new List<TweenerCore<Vector3, Vector3, VectorOptions>>();

    public async UniTask Execute()
    {
        // 登録されたUnityEventを実行
        _awake.Invoke();
        // テキストを更新
        _text.text = _messageText;
        // 移動を開始
        foreach (var e in _move)
        {
            if (e.Origin == null) continue;

            _mover.Add(e.Origin.DOMove(e.EndPos, _time).SetEase(e.Ease));
        }
        // アニメーションを再生
        foreach (var e in _animation)
        {
            if (e.Animator == null) continue;

            e.Animator.Play(e.Name);
        }
        // 待機
        await UniTask.Delay((int)(_time * 1000f));

        // DOTween をキル
        foreach (var e in _mover)
        {
            e.Kill();
        }
    }

    [Serializable]
    private class PerformanceMovement
    {
        [SerializeField]
        private Transform _origin = default;
        [SerializeField]
        private Vector3 _endPos = default;
        [SerializeField]
        private Ease _ease = default;

        public Transform Origin => _origin;
        public Vector3 EndPos => _endPos;
        public Ease Ease => _ease;
    }
    [Serializable]
    private class PerformanceAnimation
    {
        [SerializeField]
        private Animator _animator = default;
        [SerializeField]
        private string _name = default;

        public Animator Animator => _animator;
        public string Name => _name;
    }
}
