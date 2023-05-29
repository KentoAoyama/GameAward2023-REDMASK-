// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// ギャラリーで 特定の一枚絵を閲覧する為の 
/// イメージにアタッチする用のコンポーネント。
/// </summary>
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

    /// <summary>
    /// ギャラリーのメイン画像が表示されているかどうかを表す値。
    /// </summary>
    public bool MainImageVisible
    {
        get => _mainImageVisible;
        set => _mainImageVisible = value;
    }

    private void Awake()
    {
        // 最初は見えないくらい小さい
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        // ポップアップする
        transform.DOScale(Vector3.one, _openDuration).SetEase(_openEase);

        // メインイメージが表示されている間は、各ボタンを非アクティブにする
        for (int i = 0; i < _galleryController.GalleryImages.Length; i++)
        {
            _galleryController.GalleryImages[i].Button.interactable = false;
        }
    }

    private void OnDisable()
    {
        _galleryController.SetupImages();
    }

    private void Update()
    {
        // cキーあるいは、ゲームパッドのbボタンが押された事を検知する。
        bool closeInput =
            (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame);

        // メインイメージが表示されている状態 かつ 戻る入力が発生したら閉じるアニメーションを実行する。
        if (_mainImageVisible && closeInput)
        {
            CloseAnimation();
        }
    }

    public void CloseAnimation()
    {
        // アニメーション再生中は重複して再生しない
        if (!_isCloseAnimPlaying)
        {
            _isCloseAnimPlaying = true;
            transform.DOScale(Vector3.zero, _closeDuration).
                SetEase(_closeEase).OnComplete(() =>
                {
                    _mainImageVisible = false;   // このオブジェクトは見えない
                    gameObject.SetActive(false); // このオブジェクトを非アクティブにする
                    _isCloseAnimPlaying = false; // アニメーションが完了したことを知らせる
                    _menuPanelController.Cansellable = true; // ギャラリーウィンドウから、メインウィンドウへ遷移可能にする

                    for (int i = 0; i < _galleryController.GalleryImages.Length; i++)
                    {
                        _galleryController.GalleryImages[i].GetComponent<ButtonNavigationController>().Setting();
                    }
                });
        }
    }
}
