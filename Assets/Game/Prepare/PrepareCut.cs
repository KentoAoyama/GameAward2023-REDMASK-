// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PrepareCut : MonoBehaviour
{
    [SerializeField]
    private Image _iamge = default;
    [SerializeField]
    private float _delay = 1f;
    [SerializeField]
    private Sprite[] _sprites = default;

    public async void Play(int index)
    {
        if (index >= _sprites.Length || index < 0)
        {
            Debug.LogError("index が範囲外です。");
            return;
        }
        _iamge.gameObject.SetActive(true);
        _iamge.sprite = _sprites[index];
        await UniTask.Delay(TimeSpan.FromSeconds(_delay));
        _iamge.sprite = null;
        _iamge.gameObject.SetActive(false);
    }
}
