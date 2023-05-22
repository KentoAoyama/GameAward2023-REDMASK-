// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class MenuWindowManualText : MonoBehaviour
{
    [SerializeField]
    private string _keybordMouseText = default;
    [SerializeField]
    private string _gamepadText = default;
    private Text _text = default;

    private void Start()
    {
        _text = GetComponent<Text>();
    }
    private void Update()
    {
        if (MenuWindowController.CurrentDevice == PrepareDevice.GamePad)
        {
            _text.text = _gamepadText;
        }
        else
        {
            _text.text = _keybordMouseText;
        }
    }
}
