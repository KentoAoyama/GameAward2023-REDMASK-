// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 回避クラス
    /// </summary>
    [System.Serializable]
    public class Avoidance
    {
        private PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        private bool _isExecutionNow = false;
        public async void Update()
        {
            // 回避入力が発生したときに処理を実行する
            if (!_isExecutionNow &&
                _playerController.InputManager.GetValue<float>(InputType.Avoidance) > 0.49f)
            {
                _isExecutionNow = true;
                // 移動入力がある場合ローリング。
                if (_playerController.InputManager.IsExist[InputType.MoveHorizontal])
                {
                    StartRollingAvoidance();
                    // 回避が完了するまで待機する
                    // await UniTask.WaitUntil(() => true);
                    await UniTask.Delay(1000); // とりあえず一秒待つ
                    EndRollingAvoidance();
                }
                // 移動入力がない場合はその場回避。
                else
                {
                    StartThereAvoidance();
                    // 回避が完了するまで待機する
                    // await UniTask.WaitUntil(() => true);
                    await UniTask.Delay(1000); // とりあえず一秒待つ
                    EndThereAvoidance();
                }

                // 入力を開放するまで待機
                await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Avoidance) < 0.01f);
                _isExecutionNow = false;
            }
        }

        /// <summary>
        /// その場回避開始処理
        /// </summary>
        private void StartThereAvoidance()
        {
            Debug.Log("その場回避始めええええ！！！");
            _playerController.LifeController.IsGodMode = true;
        }
        /// <summary>
        /// その場回避終了処理
        /// </summary>
        private void EndThereAvoidance()
        {
            Debug.Log("その場回避終了おおおおおおお！！！");
            _playerController.LifeController.IsGodMode = false;
        }
        /// <summary>
        /// ローリング回避開始処理
        /// </summary>
        private void StartRollingAvoidance()
        {
            Debug.Log("ローリング回避始めええええ！！！");
            _playerController.LifeController.IsGodMode = true;
        }
        /// <summary>
        /// ローリング回避終了処理
        /// </summary>
        private void EndRollingAvoidance()
        {
            Debug.Log("ローリング回避終了おおおおおおお！！！");
            _playerController.LifeController.IsGodMode = false;
        }
    }
}