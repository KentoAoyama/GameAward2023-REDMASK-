// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PrepareMainManualText : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private PrepareDeviceManager _deviceManager = default;

    private void Awake()
    {
        _deviceManager.CurrentDevice.Subscribe(value =>
        {
            if (value == PrepareDevice.GamePad)
            {
                _text.text = "ここにゲームパッド操作のテキストを表示する。";
            }
            else if (value == PrepareDevice.KeyboardAndMouse)
            {
                _text.text = "ここにキーボードマウス操作のテキストを表示する。";
            }
        });
    }
}
