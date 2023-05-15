// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

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
    [SerializeField]
    private TalkSE _talkSE = default;

    private List<TweenerCore<Vector3, Vector3, VectorOptions>> _mover = new List<TweenerCore<Vector3, Vector3, VectorOptions>>();

    private bool _isFire = false;
    public bool IsFire => _isFire;

    private string _cueName = "";

    private float _timer = 0f;

    private int _index = -1;

    private string _currentText = "";

    private float _talkDelay = 0.5f;

    private enum TalkSE
    {
        None,
        Player,
        Target,
        Boss,
        Robot
    }

    public async UniTask Execute()
    {
        // 登録されたUnityEventを実行
        _awake?.Invoke();

        // テキストを更新
        Tween t = default;
        if (_text != null)
        {
            //_text.text = _messageText;
            t = _text.DOText(_messageText, _time - _talkDelay).SetEase(Ease.Linear);

            switch (_talkSE)
            {
                //case TalkSE.Player:
                //    _cueName = "SE_Talk_Player";
                //    break;
                //case TalkSE.Target:
                //    _cueName = "SE_Talk_Target";
                //    break;
                //case TalkSE.Boss:
                //    _cueName = "SE_Talk_Boss";
                //    break;
                //case TalkSE.Robot:
                //    _cueName = "SE_Talk_Robot";
                //    break;
                case TalkSE.Player:
                    _cueName = "SE_Talk_Player_solo";
                    break;
                case TalkSE.Target:
                    _cueName = "SE_Talk_Target_solo";
                    break;
                case TalkSE.Boss:
                    _cueName = "SE_Talk_Boss_solo";
                    break;
                case TalkSE.Robot:
                    _cueName = "SE_Talk_Robot_solo";
                    break;
            }
        }
        // 移動を開始
        foreach (var e in _move)
        {
            if (e.Origin == null) continue;

            _mover.Add(e.Origin.DOMove(e.EndPos.position, _time).SetEase(e.Ease));
        }
        // アニメーションを再生
        foreach (var e in _animation)
        {
            if (e.Animator == null) continue;

            e.Animator.Play(e.Name);
        }

        await UniTask.WaitUntil(() =>
        {
            if (_isWaitFireInput) // 攻撃入力を待つイベントの場合攻撃入力が発生したらこのイベントを終了する。
            {
                if (_timer > _time)
                {
                    return true;
                }
                else if (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame || // 発砲入力が発生した時
                    Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    _isFire = true;
                    return true;
                }
            }
            else // そうでない場合、タイマーを消化したらイベントを終了する。
            {
                if (_timer > _time)
                {
                    return true;
                }
            }
            return false;
        });

        // このイベントを終了する。
        FinishEvent(t);
    }

    private void FinishEvent(Tween textTween)
    {
        if (_text != null)
            _text.text = " ";

        // DOTween をキル
        textTween.Kill();
        foreach (var e in _mover)
        {
            e?.Kill();
        }
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_messageText.Length < 3 || _cueName == "" || _talkSE == TalkSE.None) return;
        
        if (_currentText != _text.text)
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", _cueName);
            _currentText = _text.text;
        }
    }

    [Serializable]
    private class PerformanceMovement
    {
        [SerializeField]
        private Transform _origin = default;
        [SerializeField]
        private Transform _endPos = default;
        [SerializeField]
        private Ease _ease = default;

        public Transform Origin => _origin;
        public Transform EndPos => _endPos;
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
