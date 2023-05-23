// 日本語対応
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MaskSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextSelectedButton = default;
    [SerializeField]
    private EventSystem _eventSystem = default;

    private Image _maskImage = null;
    private Button _button = null;
    private BoolReactiveProperty _isSet = new BoolReactiveProperty(true);

    public IReadOnlyReactiveProperty<bool> IsSet => _isSet;

    private void Awake()
    {
        _maskImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Switch);
    }
    private void Switch()
    {
        //_maskImage.color = _isSet.Value ? Color.white : Color.clear;
        //_button.interactable = false;
        //_eventSystem.SetSelectedGameObject(_nextSelectedButton);
        //_isSet.Value ^= true;
    }
}