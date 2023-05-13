// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;

public class PrepareCut : MonoBehaviour
{
    [SerializeField]
    private Image _iamge = default;
    [SerializeField]
    private Image _fadePanel = default;
    [SerializeField]
    private Sprite[] _sprites = default;
    [Space, Header("AnimDelay")]
    [SerializeField]
    private float _firstFadeDelay = 1f;
    [SerializeField]
    private float _inputDelay = 0.5f;

    private int _amountId = Shader.PropertyToID("_Amount");
    private InputAction _allButtons = new InputAction(binding: "*/<Button>");

    private void Awake()
    {
        _allButtons.Enable();
        Play(GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber());
    }

    public async void Play(int index)
    {
        if (index >= _sprites.Length || index < 0)
        {
            Debug.LogError("index が範囲外です。");
            return;
        }
        _iamge.gameObject.SetActive(true);
        _iamge.sprite = _sprites[index];
        _iamge.material.SetFloat(_amountId, -1F);
        _fadePanel.color = Color.black;

        await _fadePanel.DOColor(new Color(0.9622642F, 0.8783826F, 0.7571022F, 1), 2F);
        await _fadePanel.DOFade(0F, 0.1F);
        await DOTween.To(() => -1F, x => _iamge.material.SetFloat(_amountId, x), 1f, _firstFadeDelay);
        await UniTask.Delay(TimeSpan.FromSeconds(_inputDelay));
        await UniTask.WaitUntil(() => _allButtons.IsPressed());
        var temp = DOTween.To(() => 1F, x => _iamge.material.SetFloat(_amountId, x), -1f, _firstFadeDelay);
        var temp2 = _iamge.DOFade(0f, _firstFadeDelay);
        await UniTask.WhenAll(temp.AsyncWaitForCompletion().AsUniTask(), temp2.AsyncWaitForCompletion().AsUniTask());
        _iamge.sprite = null;
        _iamge.gameObject.SetActive(false);
    }
}
