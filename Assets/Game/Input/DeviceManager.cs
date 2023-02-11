using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    /// 使用デバイスを制御するクラス
    /// </summary>
    public class DeviceManager
    {
        private Device _currentDevice;

        public Device CurrentDevice => _currentDevice;

        public void Update()
        {
            // Debug.Log(_currentDevice); // どちらを使用しているか確認する用
            // クリックあるいはキーボードによっていずれかの入力が発生した時の処理
            if (IsKeyboardAndMouseInput()) _currentDevice = Device.KeyboardAndMouse;
            // ゲームパッドによっていずれかの入力が発生した時の処理
            else if (IsGamePadInput()) _currentDevice = Device.GamePad;
        }
        /// <summary>
        /// キーボード、マウスのいずれかのボタンが押下されたときtrueを返す
        /// </summary>
        /// <returns></returns>
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

                    Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.8f ||
                    Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.8f;
            }
            return false;
        }
    }
    public enum Device
    {
        KeyboardAndMouse,
        GamePad
    }
}