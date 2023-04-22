// 日本語対応
using UnityEngine;

public class GalleryMainImage : MonoBehaviour
{
    [SerializeField]
    private GalleryController _galleryController = default;

    private void OnEnable()
    {
        for (int i = 0; i < _galleryController.GalleryImages.Length; i++)
        {
            _galleryController.GalleryImages[i].Button.enabled = false;
        }
    }
    private void OnDisable()
    {
        _galleryController.SetupImages();
    }
}
