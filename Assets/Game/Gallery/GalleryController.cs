// 日本語対応
using UnityEngine;

/// <summary>
/// ギャラリー画面を制御するコンポーネント
/// ギャラリーオブジェクトのルートオブジェクトにアタッチすることを想定している。
/// </summary>
public class GalleryController : MonoBehaviour
{
    [SerializeField]
    private GalleryButton[] _galleryImages = default;

    public GalleryButton[] GalleryImages => _galleryImages;

    private void OnEnable()
    {
        SetupImages();
    }
    public void SetupImages()
    {
        // ギャラリー画面に配置された各ボタンのセットアップを行う
        for (int i = 0; i < _galleryImages.Length; i++)
        {
            _galleryImages[i].Setup(GameManager.Instance.GalleryManager.IsOpenedID(_galleryImages[i].ID));
        }
    }
}