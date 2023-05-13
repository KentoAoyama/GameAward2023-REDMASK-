// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuWindowStartUpButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuWindow = default;

    private void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            // Keyboardの Escape keyが押下された時か
            // XboxコントローラのOptionボタンが押された場合の処理
            _menuWindow.SetActive(true);
        }
    }
}
