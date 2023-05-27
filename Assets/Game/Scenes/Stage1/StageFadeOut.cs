// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageFadeOut : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;
    [SerializeField]
    private float _time = default;
    [SerializeField]
    private Ease _ease = default;
    [SerializeField]
    private UnityEvent _startAction = default;
    [SerializeField]
    private UnityEvent _onComplete = default;

    public async UniTask FadeOut(Action startAction = null, Action onComplete = null)
    {
        startAction?.Invoke();
        _startAction.Invoke();
        await _targetImage.DOFade(1f, _time).SetEase(_ease);
        onComplete?.Invoke();
        _onComplete.Invoke();
    }
}
