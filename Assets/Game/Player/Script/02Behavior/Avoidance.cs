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
        [Tooltip("回避中の時間の速度"), SerializeField]
        private float _timeScale = 0.7f;

        [Tooltip("ロール回避の移動速度"), SerializeField]
        private float _rollSpeed = 8f;


        private PlayerController _playerController = null;

        private int _myLayerIndex = default;
        private int _ignoreLayerIndex = default;

        /// <summary>ロールで回避する方向</summary>
        private float _roalDir = 1;

        /// <summary>現在、回避をしているかどうかを示す</summary>
        private bool _isAvoidacneNow = false;

        private bool _isRollAvoidance = false;

        public bool IsAvoiddanceNow => _isAvoidacneNow;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            // 自分と無視するレイヤーのインデックスを取得
            _ignoreLayerIndex = LayerMask.NameToLayer("EnemyBullet"); // ここの文字列を消したい
            _myLayerIndex = LayerMask.NameToLayer("Player"); // ここの文字列を消したい
        }
        private bool _isExecutionNow = false;
        public async void Update()
        {
            if (_isRollAvoidance)
            {
                _playerController.Rigidbody2D.velocity =
                    new Vector2(_playerController.Player.transform.localScale.x * _rollSpeed, _playerController.Rigidbody2D.velocity.y);
            }


            // 回避入力が発生したときに処理を実行する
            if (!_isExecutionNow &&
                _playerController.InputManager.GetValue<float>(InputType.Avoidance) > 0.49f
                && _playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                _isExecutionNow = true;
                // 移動入力がある場合ローリング。
                if (_playerController.InputManager.IsExist[InputType.MoveHorizontal])
                {
                    //回避方向移動入力を取得
                    _roalDir = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal);
                    _playerController.PlayerAnimatorControl.SetPlayerDir(_roalDir);

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
            Debug.Log("その場回避始め！");
            _playerController.LifeController.IsGodMode = true;
            Physics2D.IgnoreLayerCollision(_ignoreLayerIndex, _myLayerIndex, true);
            // 時間の速度をゆっくりにする。
            GameManager.Instance.TimeController.ChangeTimeSpeed(_timeScale);

            _isAvoidacneNow = true;
        }
        /// <summary>
        /// その場回避終了処理
        /// </summary>
        private void EndThereAvoidance()
        {
            Debug.Log("その場回避終了！");
            _playerController.LifeController.IsGodMode = false;
            Physics2D.IgnoreLayerCollision(_ignoreLayerIndex, _myLayerIndex, false);
            // 時間の速度をもとの状態に戻す。
            GameManager.Instance.TimeController.ChangeTimeSpeed(1f);

            _isAvoidacneNow = false;
        }
        /// <summary>
        /// ローリング回避開始処理
        /// </summary>
        private void StartRollingAvoidance()
        {
            Debug.Log("ローリング回避始め！");
            _playerController.LifeController.IsGodMode = true;
            _isAvoidacneNow = true;
            _isRollAvoidance = true;

        }
        /// <summary>
        /// ローリング回避終了処理
        /// </summary>
        private void EndRollingAvoidance()
        {
            Debug.Log("ローリング回避終了！");
            _playerController.LifeController.IsGodMode = false;
            _isAvoidacneNow = false;
            _isRollAvoidance = false;

            _playerController.Move.EndAvoidanceInout(_playerController.InputManager.GetValue<float>(InputType.MoveHorizontal));
        }
    }
}