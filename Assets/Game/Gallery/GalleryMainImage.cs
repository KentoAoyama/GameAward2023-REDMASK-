// 日本語対応
using UnityEngine;
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

    private bool _isCloseAnimPlaying = false;

    private void Awake()
    {
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
    public void CloseAnimatin()
    {
        if (!_isCloseAnimPlaying)
        {
            _isCloseAnimPlaying = true;
            transform.DOScale(Vector3.zero, _closeDuration).
                SetEase(_closeEase).OnComplete(() =>
                { gameObject.SetActive(false); _isCloseAnimPlaying = false; });
        }
    }
}
