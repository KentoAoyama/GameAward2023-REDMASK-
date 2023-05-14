// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GalleryMainImage : MonoBehaviour
{
    [SerializeField]
    private GalleryController _galleryController = default;
    [Header("表示時アニメーション")]
    [SerializeField]
    private float _openDuration = default;
    [SerializeField]
    private Ease _openEase = default;

    [Header("非表示時アニメーション")]
    [SerializeField]
    private float _closeDuration = default;
    [SerializeField]
    private Ease _closeEase = default;
    [SerializeField]
    private MenuPanelController _menuPanelController = default;

    private bool _isCloseAnimPlaying = false;
    private bool _mainImageVisible = false;
    public bool MainImageVisible
    {
        get => _mainImageVisible;
        set => _mainImageVisible = value;
    }

    private void Awake()
    {
        //_mainImageVisible = false;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        transform.DOScale(Vector3.one, _openDuration).SetEase(_openEase);
        for (int i = 0; i < _galleryController.GalleryImages.Length; i++)
        {
            _galleryController.GalleryImages[i].Button.enabled = false;
        }
    }

    private void OnDisable()
    {
        _galleryController.SetupImages();
    }

    private void Update()
    {
        var close = Keyboard.current.cKey.wasPressedThisFrame;
        if (Gamepad.current != null)
        {
            close |= Gamepad.current.bButton.wasPressedThisFrame;
        }

        if (_mainImageVisible && close)
        {
            CloseAnimation();
        }    
    }

    public void CloseAnimation()
    {
        if (!_isCloseAnimPlaying)
        {
            _mainImageVisible = false;
            _isCloseAnimPlaying = true;
            transform.DOScale(Vector3.zero, _closeDuration).
                SetEase(_closeEase).OnComplete(() =>
                { gameObject.SetActive(false); _isCloseAnimPlaying = false; _menuPanelController.Cansellable = true; });
        }
    }
}
