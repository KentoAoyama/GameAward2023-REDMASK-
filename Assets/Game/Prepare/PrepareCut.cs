// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

public class PrepareCut : MonoBehaviour
{
    [SerializeField]
    private Image _iamge = default;
    [SerializeField]
    private Image _fadePanel = default;
    [SerializeField]
    private KeySpritePair[] _sprites = default;
    [Space, Header("AnimDelay")]
    [SerializeField]
    private float _firstFadeDelay = 1f;
    [SerializeField]
    private float _inputDelay = 0.5f;
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private int _testIndex = -1;

    /// <summary>カットシーンが終わっているか</summary>
    private bool _cutSceneEnded = false;
    private int _amountId = Shader.PropertyToID("_Amount");
    private InputAction _allButtons = new InputAction(binding: "*/<Button>");

    public bool CutSceneEnded => _cutSceneEnded;

    private int _noiseSEIndex = -1;
    

    private void Awake()
    {
        _cutSceneEnded = false;
        _allButtons.Enable();
        Play(GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber());
        _text.color = new Color(1, 1, 1, 0);
    }

    public async void Play(int index)
    {
        if (_testIndex > -1)
        {
            index = _testIndex;
        }

        if (index == 3)
        {
            index = 5;
        }

        if (index < 0)
        {
            Debug.LogError("index が範囲外です。");
            return;
        }
        _iamge.gameObject.SetActive(true);
        var spriteData = Array.Find(_sprites, x => x.Key == index);

        if (spriteData != null)
        {
            _iamge.sprite = spriteData.Sprite;
            _iamge.color = spriteData.Color;
        }

        _iamge.material.SetFloat(_amountId, -1F);
        _fadePanel.color = Color.black;

        _noiseSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Noise");

        await _fadePanel.DOColor(new Color(0.9622642F, 0.8783826F, 0.7571022F, 1), 2F);
        await _fadePanel.DOFade(0F, 0.1F);
        await DOTween.To(() => -1F, x => _iamge.material.SetFloat(_amountId, x), 1f, _firstFadeDelay);
        await UniTask.Delay(TimeSpan.FromSeconds(_inputDelay));

        var text = _text.DOFade(1.0F, 1.0F);
        var press = UniTask.WaitUntil(() => _allButtons.IsPressed());
        await UniTask.WhenAll(text.AsyncWaitForCompletion().AsUniTask(), press);

        var textFade = _text.DOFade(0.0F, 0.3F);
        var temp = DOTween.To(() => 1F, x => _iamge.material.SetFloat(_amountId, x), -1f, _firstFadeDelay);
        var temp2 = _iamge.DOFade(0f, _firstFadeDelay);
        await UniTask.WhenAll(temp.AsyncWaitForCompletion().AsUniTask(), temp2.AsyncWaitForCompletion().AsUniTask(), textFade.AsyncWaitForCompletion().AsUniTask());

        GameManager.Instance.AudioManager.StopSE(_noiseSEIndex);

        _text.gameObject.SetActive(false);
        _iamge.sprite = null;
        _iamge.gameObject.SetActive(false);
        _cutSceneEnded = true;

        GameManager.Instance.GalleryManager.SetOpenedID(true, index);
    }

    [Serializable]
    class KeySpritePair
    {
        [SerializeField]
        private int key = default(int);
        [SerializeField]
        private Sprite sprite = default;
        [SerializeField]
        private Color color = Color.white;

        public int Key 
        { 
            get => key;
            set => key = value;
        }

        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }
    }
}