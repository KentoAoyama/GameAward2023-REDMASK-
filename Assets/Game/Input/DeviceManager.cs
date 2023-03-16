using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

namespace Input
{
    /// <summary>
    /// 使用デバイスを制御するクラス
    /// </summary>
    public class DeviceManager
    {
        private ReactiveProperty<Device> _currentDevice = new ReactiveProperty<Device>(Device.KeyboardAndMouse);

        /// <summary>
        /// 現在の使用デバイスを表現する値
        /// </summary>
        public IReadOnlyReactiveProperty<Device> CurrentDevice => _currentDevice;

        public async void Update()
        {
            try
            {
                // Debug.Log(_currentDevice); // 現在の使用デバイスを確認する用
                // クリックあるいはキーボードによっていずれかの入力が発生した時の処理
                if (IsKeyboardAndMouseInput() && _currentDevice.Value == Device.GamePad)
                {
                    _currentDevice.Value = Device.Switching;
                    await UniTask.DelayFrame(1);
                    _currentDevice.Value = Device.KeyboardAndMouse;
                }
                // ゲームパッドによっていずれかの入力が発生した時の処理
                else if (IsGamePadInput() && _currentDevice.Value == Device.KeyboardAndMouse)
                {
                    _currentDevice.Value = Device.Switching;
                    await UniTask.DelayFrame(1);
                    _currentDevice.Value = Device.GamePad;
                }
            }
            catch (MissingReferenceException)
            {

            }
        }
        /// <summary>
        /// キーボード、マウスのいずれかのボタンが押下されたときtrueを返す
        /// </summary>
        private bool IsKeyboardAndMouseInput()
        {
            return
                // キーボード
                Keyboard.current.anyKey.wasPressedThisFrame ||
                // マウス
                Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame ||
                Mouse.current.forwardButton.wasPressedThisFrame ||
                Mouse.current.backButton.wasPressedThisFrame;
        }
        /// <summary>
        /// ゲームパッドのいずれかのボタンが押下されたときtrueを返す
        /// </summary>
        private bool IsGamePadInput()
        {
            if (Gamepad.current != null) // ゲームパッドが接続されているかどうかチェックする
            {
                return
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

                    Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.8f || // 接続した状態で実行すると1フレーム目で0.99999を返すのでGamePadスタートになる
                    Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.8f;  // 同上
            }
            return false;
        }
    }
    public enum Device
    {
        /// <summary> 切り替え中 </summary>
        Switching,
        /// <summary> キーボード, マウス </summary>
        KeyboardAndMouse,
        /// <summary> ゲームパッド </summary>
        GamePad
    }
}