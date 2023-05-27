// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrepareFadeOut : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;
    [SerializeField]
    private Vector2 _endPos = default;
    [SerializeField]
    private float _time = default;
    [SerializeField]
    private Ease _ease = default;
    [SerializeField]
    private UnityEvent _startEvent = default;
    [SerializeField]
    private UnityEvent _onComplete = default;

    public async UniTask FadeOut(Action startEvent = null, Action onComplete = null)
    {
        _startEvent.Invoke();
        startEvent?.Invoke();
        await _targetImage.rectTransform.DOAnchorPos(_endPos, _time).SetEase(_ease);
        _onComplete.Invoke();
        onComplete?.Invoke();
    }
}
