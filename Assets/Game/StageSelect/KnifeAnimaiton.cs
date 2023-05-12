// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KnifeAnimaiton : MonoBehaviour
{
    [SerializeField]
    private Vector2 _endPos = default;
    [SerializeField]
    private float _time = 1.2f;
    [SerializeField]
    private Ease _ease = default;

    public event Action OnComplete = default;

    private RectTransform _rectTransform = default;

    //Vector2 firstPos = default;

    private void Awake()
    {
        //firstPos = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        //if (Keyboard.current.gKey.wasPressedThisFrame) await Play();
        //if (Keyboard.current.hKey.wasPressedThisFrame) _rectTransform.anchoredPosition = firstPos;
    }

    public async UniTask Play(Action onComplete)
    {
        _rectTransform = GetComponent<RectTransform>();
        await _rectTransform.DOAnchorPos(_endPos, _time).
            SetEase(_ease).
            OnComplete(() => onComplete?.Invoke());
    }
}
