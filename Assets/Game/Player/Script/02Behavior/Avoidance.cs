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

        [Header("減速値")]
        [Tooltip("減速度（地上）"), SerializeField]
        private float _landDeceleration = 20f;

        [Tooltip("現在の速度 : インスペクタで値を追跡する用"), SerializeField]
        private float _currentHorizontalSpeed = 0f;

        private PlayerController _playerController = null;

        private int _myLayerIndex = default;
        private int _ignoreLayerIndex = default;

        /// <summary>現在、回避をしているかどうかを示す</summary>
        private bool _isAvoidacneNow = false;

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
            // 回避入力が発生したときに処理を実行する
            if (!_isExecutionNow &&
                _playerController.InputManager.GetValue<float>(InputType.Avoidance) > 0.49f
                && _playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                _isExecutionNow = true;

                StartThereAvoidance();
                // 回避が完了するまで待機する
                // await UniTask.WaitUntil(() => true);
                await UniTask.Delay(1000); // とりあえず一秒待つ
                EndThereAvoidance();

                // 入力を開放するまで待機
                await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Avoidance) < 0.01f);
                _isExecutionNow = false;
            }


            if (_isAvoidacneNow)
            {
                _currentHorizontalSpeed = _playerController.Rigidbody2D.velocity.x;

                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    _currentHorizontalSpeed -= Time.deltaTime * _landDeceleration * _playerController.Player.transform.localScale.x;
                } // 地上移動中の減速処理

                if (_playerController.Player.transform.localScale.x > 0f && _currentHorizontalSpeed < 0f ||
                    _playerController.Player.transform.localScale.x < 0f && _currentHorizontalSpeed > 0f)
                {
                    _currentHorizontalSpeed = 0f;
                } // 「右」に向いている状態で減速するときは0より小さくならない、
                  // 「左」に向いている状態で減速するときは0より大きくならない。


                // 速度を割り当てる。
                _playerController.Rigidbody2D.velocity =
                        new Vector2(_currentHorizontalSpeed,
                        _playerController.Rigidbody2D.velocity.y);
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

            //回避が終了したことをMoveクラスに伝える
            _playerController.Move.EndAvoidance();

            _isAvoidacneNow = false;
        }
    }
}