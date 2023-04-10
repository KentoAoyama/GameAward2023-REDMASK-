// 日本語対応
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class MaskSwitch : MonoBehaviour
{
    private Image _maskImage = null;
    private BoolReactiveProperty _isSet = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsSet => _isSet;

    private void Awake()
    {
        _maskImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(Switch);
    }
    private void Switch()
    {
        _maskImage.color = _isSet.Value ? Color.white : Color.clear;
        _isSet.Value ^= true;
    }
}