//日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class PrepareDeviceManager : MonoBehaviour
{
    private ReactiveProperty<PrepareDevice> _currentDevice =
        new ReactiveProperty<PrepareDevice>(PrepareDevice.KeyboardAndMouse);

    public IReadOnlyReactiveProperty<PrepareDevice> CurrentDevice => _currentDevice;

    public void Update()
    {
        // 現在扱っているデバイスに変更があった場合,プロパティを更新する。
        var input = MonitorInput();
        if (input != PrepareDevice.None) _currentDevice.Value = input;
    }
    private PrepareDevice MonitorInput()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current != null && (
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame ||
            Mouse.current.middleButton.wasPressedThisFrame ||
            Mouse.current.forwardButton.wasPressedThisFrame ||
            Mouse.current.backButton.wasPressedThisFrame))
        {
            return PrepareDevice.KeyboardAndMouse;
        }
        if (Gamepad.current != null && (
            // 〇△□×,ABCDボタン
            Gamepad.current.buttonSouth.wasPressedThisFrame ||
            Gamepad.current.buttonNorth.wasPressedThisFrame ||
            Gamepad.current.buttonWest.wasPressedThisFrame ||
            Gamepad.current.buttonEast.wasPressedThisFrame ||
            // スティック押し込み
            Gamepad.current.leftStickButton.wasPressedThisFrame ||
            Gamepad.current.rightStickButton.wasPressedThisFrame ||
            // スタート、セレクトボタン
            Gamepad.current.selectButton.wasPressedThisFrame ||
            Gamepad.current.startButton.wasPressedThisFrame ||
            // 十字キー
            Gamepad.current.dpad.left.wasPressedThisFrame ||
            Gamepad.current.dpad.right.wasPressedThisFrame ||
            Gamepad.current.dpad.up.wasPressedThisFrame ||
            Gamepad.current.dpad.down.wasPressedThisFrame ||
            // RB,LB,RT,LT
            Gamepad.current.rightShoulder.wasPressedThisFrame ||
            Gamepad.current.leftShoulder.wasPressedThisFrame ||
            Gamepad.current.rightTrigger.wasPressedThisFrame ||
            Gamepad.current.leftTrigger.wasPressedThisFrame ||

            Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.8f ||
            Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.8f))
        {
            return PrepareDevice.GamePad;
        }

        return PrepareDevice.None;
    }
}
public enum PrepareDevice
{
    KeyboardAndMouse,
    GamePad,
    None
}