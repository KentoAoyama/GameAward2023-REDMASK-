// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class GalleryButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _sprite = default;
    [SerializeField]
    private Sprite _nonOpenedSprite = default;
    [SerializeField]
    private Image _mainImage = default;
    [SerializeField]
    private int _id = default;
    [SerializeField]
    private MenuPanelController _menuPanelController = default;

    private Button _button = null;
    private Image _image = null;

    public Button Button => _button;
    public int ID => _id;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }
    public void Setup(bool isOpened)
    {
        (_button ?? GetComponent<Button>()).interactable = isOpened;

        if (isOpened)
        {
            (_image ?? GetComponent<Image>()).sprite = _sprite;
        }
        else
        {
            (_image ?? GetComponent<Image>()).sprite = _nonOpenedSprite;
        }
    }
    private void OnButtonClick()
    {
        _mainImage.GetComponent<GalleryMainImage>().MainImageVisible = true;
        _mainImage.sprite = _sprite;
        _mainImage.gameObject.SetActive(true);
        _menuPanelController.Cansellable = false;
    }
    private void Return()
    {
        _mainImage.gameObject.SetActive(false);
    }
}