// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StageFadeIn : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;
    [SerializeField]
    private float _time = default;
    [SerializeField]
    private Ease _ease = default;

    public async void FadeIn(Action startAction = null, Action onComplete = null)
    {
        startAction?.Invoke();
        await _targetImage.DOFade(0f, _time).SetEase(_ease);
        onComplete?.Invoke();
    }
}
