// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuResumeButton : MonoBehaviour
{
    private void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            gameObject.SetActive(false);
        }
    }
}
