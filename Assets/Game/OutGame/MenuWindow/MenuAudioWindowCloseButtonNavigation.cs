// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAudioWindowCloseButtonNavigation : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem = default;
    [Header("左入力が発生したときの遷移先ボタン")]
    [SerializeField]
    private Selectable _leftButton = default;

    private Button _thisButton = null;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();

        // get the Navigation data
        Navigation navigation = _thisButton.navigation;
        navigation.mode = Navigation.Mode.Explicit;

        // 上の設定
        navigation.selectOnUp = _thisButton;
        // 下の設定
        navigation.selectOnDown = _thisButton;
        // 右を設定
        navigation.selectOnRight = _thisButton;
        // 左を設定
        navigation.selectOnLeft = _leftButton;

        _thisButton.navigation = navigation;
    }
}
