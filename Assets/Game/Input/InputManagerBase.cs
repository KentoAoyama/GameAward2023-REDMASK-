using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    /// 制作者(丸岡)がInputSystemを理解することと
    /// InputSystemをあまり知らない人でも利用しやすいようにすることを目的に作製したクラス。
    /// 利用例を InputManager, InputTestクラスに示す。
    /// </summary>
    /// <typeparam name="TController"> Unity上で生成したInputActionsファイルのC#スクリプトの型を割り当てる </typeparam>
    /// <typeparam name="TEnum"> 入力の種類を表す列挙型 </typeparam>
    public abstract class InputManagerBase<TController, TEnum>
        where TController : IInputActionCollection2, new()
        where TEnum : Enum
    {
        /// <summary> InputSystemが生成したActionファイル </summary>
        protected TController _inputActionCollection = default;
        /// <summary> 入力の種類をキーに渡す事で、対応するInputActionにアクセスできる </summary>
        private Dictionary<TEnum, InputAction> _inputActions = new Dictionary<TEnum, InputAction>();
        private Dictionary<TEnum, bool> _isPressed = new Dictionary<TEnum, bool>();
        private Dictionary<TEnum, bool> _isExist = new Dictionary<TEnum, bool>();
        private Dictionary<TEnum, bool> _isReleased = new Dictionary<TEnum, bool>();
        /// <summary>
        /// 特定のボタンの現在の値を保存するDictionary。
        /// GetValue()で値を取得可能。
        /// </summary>
        private Dictionary<TEnum, object> _inputValues = new Dictionary<TEnum, object>();

        /// <summary> 特定のボタンを押下したときにtrueを返すディクショナリ </summary>
        public ReadOnlyDictionary<TEnum, bool> IsPressed = null;
        /// <summary> 特定のボタンを押下中, trueを返すディクショナリ </summary>
        public ReadOnlyDictionary<TEnum, bool> IsExist = null;
        /// <summary> 特定のボタンを開放したときにtrueを返すディクショナリ </summary>
        public ReadOnlyDictionary<TEnum, bool> IsReleased = null;

        /// <summary> SetAction<>()を呼び出して,各アクションをセットしてください </summary>
        protected abstract void Setup();

        /// <summary> このクラスの初期化処理 </summary>
        public void Init()
        {
            // InputSystemを確保し,起動する。
            _inputActionCollection = new TController();
            _inputActionCollection.Enable();
            Setup();
            IsPressed = new ReadOnlyDictionary<TEnum, bool>(_isPressed);
            IsExist = new ReadOnlyDictionary<TEnum, bool>(_isExist);
            IsReleased = new ReadOnlyDictionary<TEnum, bool>(_isReleased);
        }

        /// <summary> アクションのセットアップ処理 </summary>
        /// <typeparam name="ValueType"> 受け取る入力の型 </typeparam>
        /// <param name="action"> "type"に対応するInputAction </param>
        /// <param name="type"> 入力の種類 </param>
        protected void SetAction<ValueType>(InputAction action, TEnum type) where ValueType : struct
        {
            try
            {
                // アクションをディクショナリに登録する
                _inputActions.Add(type, action);
                // ボタンの押下状態を監視するディクショナリのセットアップ
                _isPressed.Add(type, false);
                _isExist.Add(type, false);
                _isReleased.Add(type, false);
                // 値保存用のDictionaryのセットアップ
                _inputValues.Add(type, (ValueType)default);
            }
            catch (ArgumentException e)
            {
                Debug.LogError("キーが重複しています！修正してください！");
                Debug.LogError(e.Message);
                return;
            }
            // 押下時のみDictionaryがtrueを返すようにする処理を登録する
            action.started += async _ =>
            {
                _isPressed[type] = true;
                await UniTask.DelayFrame(1);
                _isPressed[type] = false;
            };

            // 押下中にDictionaryがtrueを返すようにする処理を登録する
            action.started += _ => _isExist[type] = true;
            action.canceled += _ => _isExist[type] = false;

            // ボタン解放時にDictionaryがtrueを返すようにする処理を登録する
            action.canceled += async _ =>
            {
                _isReleased[type] = true;
                await UniTask.DelayFrame(1);
                _isReleased[type] = false;
            };
            // 値の変化を追跡する処理を登録する
            action.started +=
                context => _inputValues[type] = context.ReadValue<ValueType>();
            action.performed +=
                context => _inputValues[type] = context.ReadValue<ValueType>();
            action.canceled +=
                context => _inputValues[type] = context.ReadValue<ValueType>();
        }

        /// <summary>
        /// 入力が発生した時に実行する処理を"登録"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void AddInputEnter(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].started += inputAction;
        }
        /// <summary>
        /// 入力が変化した時に実行する処理を"登録"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void AddInputStay(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].performed += inputAction;
        }
        /// <summary>
        /// 入力がなくなった時に実行する処理を"登録"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void AddInputExit(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].canceled += inputAction;
        }

        /// <summary>
        /// 入力が発生した時に実行する処理を"解除"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void RemoveInputEnter(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].started -= inputAction;
        }
        /// <summary>
        /// 入力が変化した時に実行する処理を"解除"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void RemoveInputStay(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].performed -= inputAction;
        }
        /// <summary>
        /// 入力がなくなった時に実行する処理を"解除"する
        /// </summary>
        /// <param name="type"> 入力の種類 </param>
        /// <param name="inputAction"> 実行するメソッド </param>
        public void RemoveInputExit(TEnum type, Action<InputAction.CallbackContext> inputAction)
        {
            _inputActions[type].canceled -= inputAction;
        }
        /// <summary> 指定された入力の種類に対応する値を取得する。 </summary>
        /// <typeparam name="T"> 受け取りたい型 </typeparam>
        /// <param name="type"> 入力の種類 </param>
        /// <returns></returns>
        public virtual T GetValue<T>(TEnum type)
        {
            try
            {
                return (T)_inputValues[type];
            }
            catch (InvalidCastException)
            {
                Debug.LogError($"指定された型 {typeof(ValueType).Name} が、予期される値の型と一致しません！修正してください！");
                return (T)default;
            }
        }
    }
}