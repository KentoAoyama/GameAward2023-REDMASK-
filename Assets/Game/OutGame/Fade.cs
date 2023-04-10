// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;

    // 明転する
    public void FadeIn(float duration, Action onComplete = null)
    {
        _image.DOFade(0f, duration).OnComplete(() => onComplete?.Invoke());
    }
    // 暗転する
    public void FadeOut(float duration, Action onComplete = null)
    {
        _image.DOFade(1f, duration).OnComplete(() => onComplete?.Invoke());
    }
}