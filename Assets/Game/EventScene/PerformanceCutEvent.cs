// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;

public class PerformanceCutEvent : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;
    [SerializeField]
    private Image _fadePanel = default;
    [SerializeField]
    private Sprite[] _sprites = default;
    [Space, Header("AnimDelay")]
    [SerializeField]
    private float _colorChangeInterval = 1f;
    [SerializeField]
    private float _firstFadeDelay = 1f;
    [SerializeField]
    private float _interval = 0.5f;

    /// <summary>カットシーンが終わっているか</summary>
    private bool _cutSceneEnded = false;
    private int _amountId = Shader.PropertyToID("_Amount");
    private InputAction _allButtons = new InputAction(binding: "*/<Button>");

    public bool CutSceneEnded => _cutSceneEnded;


    private void Awake()
    {
        _cutSceneEnded = false;
        _allButtons.Enable();
        _image.gameObject.SetActive(false);
    }

    public async void Play(int index)
    {
        if (index >= _sprites.Length || index < 0)
        {
            Debug.LogError("index が範囲外です。");
            return;
        }
        _image.gameObject.SetActive(true);
        _image.sprite = _sprites[index];
        _image.material.SetFloat(_amountId, -1F);
        _fadePanel.color = Color.black;
        int noise = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Noise");

        await _fadePanel.DOColor(new Color(0.9622642F, 0.8783826F, 0.7571022F, 1), _colorChangeInterval);
        await _fadePanel.DOFade(0F, 0.1F);
        await DOTween.To(() => -1F, x => _image.material.SetFloat(_amountId, x), 1f, _firstFadeDelay);
        await UniTask.Delay(TimeSpan.FromSeconds(_interval));
        var temp = DOTween.To(() => 1F, x => _image.material.SetFloat(_amountId, x), -1f, _firstFadeDelay);
        var temp2 = _image.DOFade(0f, _firstFadeDelay);
        await UniTask.WhenAll(temp.AsyncWaitForCompletion().AsUniTask(), temp2.AsyncWaitForCompletion().AsUniTask());
        _image.sprite = null;
        _image.gameObject.SetActive(false);
        _cutSceneEnded = true;

        GameManager.Instance.AudioManager.StopSE(noise);

        GameManager.Instance.GalleryManager.SetOpenedID(true, index + 3);
    }
}
