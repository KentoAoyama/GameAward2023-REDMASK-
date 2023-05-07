using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class TipsAnimation : MonoBehaviour
{
    [SerializeField]
    private string _gamepadText = "";
    [SerializeField]
    private string _keyboardText = "";
    private Text _tipsText = default;
    private string _currentText = "";
    private InputAction _gamepad = new InputAction(binding: "<Gamepad>/*");
    private InputAction _mouse = new InputAction(binding: "<Mouse>/*");

    private void Awake()
    {
        _tipsText = GetComponent<Text>();
        _gamepad.Enable();
        _mouse.Enable();
    }

    private void Update()
    {
        if (_gamepad.WasPressedThisFrame() &&
            _currentText != _gamepadText
        )
        {
            ChangeTips(_gamepadText);
        }
        else if (Keyboard.current.anyKey.wasPressedThisFrame ||
            _mouse.WasPressedThisFrame() &&
            _currentText != _keyboardText
        )
        {
            ChangeTips(_keyboardText);
        }
    }

    public void ChangeTips(string text)
    {
        _currentText = text;
        _tipsText.DOText(text, 1.0f, scrambleMode: ScrambleMode.Uppercase).SetEase(Ease.Linear);
    }
}
