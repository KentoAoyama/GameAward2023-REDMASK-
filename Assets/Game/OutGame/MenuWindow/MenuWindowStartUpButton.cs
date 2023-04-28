// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuWindowStartUpButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject _menuWindow = default;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 左クリックされた場合の処理
            _menuWindow.SetActive(true);
        }
    }

    private void Update()
    {
        if (Gamepad.current == null) return;
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            // XboxコントローラのOptionボタンが押された場合の処理
            _menuWindow.SetActive(true);
        }
    }
}
