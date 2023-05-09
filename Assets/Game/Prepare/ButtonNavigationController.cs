// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonNavigationController : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem = default;
    [Header("上入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Button[] _upButtons = default;
    [Header("下入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Button[] _downButtons = default;
    [Header("右入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Button[] _rightButtons = default;
    [Header("左入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Button[] _leftButtons = default;

    private Button _thisButton = null;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(async () => { await UniTask.DelayFrame(1); Setting(); });
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
        // 右を設定
        navigation.selectOnRight = SearchInteractableButton(_rightButtons, _thisButton);
        // 左を設定
        navigation.selectOnLeft = SearchInteractableButton(_leftButtons, _thisButton);

        _thisButton.navigation = navigation;
    }
    private Button SearchInteractableButton(Button[] buttons, Button origin)
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
