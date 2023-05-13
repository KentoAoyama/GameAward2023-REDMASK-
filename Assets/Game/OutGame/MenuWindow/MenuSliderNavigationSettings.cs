// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuSliderNavigationSettings : MonoBehaviour
{
    [Header("上入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Selectable[] _upButtons = default;
    [Header("下入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Selectable[] _downButtons = default;
    [SerializeField]
    private Selectable _closeButton = default;

    private Slider _thisButton = null;

    private void Awake()
    {
        _thisButton = GetComponent<Slider>();
        Setting();
    }
    private void Update()
    {
        // 決定ボタンを押したらCloseButtonに遷移する
        if ((Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
        {
            EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
        }
    }
    public void Setting()
    {
        // get the Navigation data
        Navigation navigation = _thisButton.navigation;
        navigation.mode = Navigation.Mode.Explicit;

        // 上の設定
        navigation.selectOnUp = SearchInteractableButton(_upButtons, _thisButton);
        // 下の設定
        navigation.selectOnDown = SearchInteractableButton(_downButtons, _thisButton);

        _thisButton.navigation = navigation;
    }
    private Selectable SearchInteractableButton(Selectable[] buttons, Selectable origin)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].interactable)
            {
                return buttons[i];
            }
        }
        return origin;
    }
}
