using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Move
    {
        [Header("水平移動")]
        [Header("地上 : 移動開始")]
        [Tooltip("低速移動速度"), SerializeField]
        private float _startSpeed = 1f;
        [Tooltip("標準移動に切り替えるまでの時間"), SerializeField]
        private float _toMoveTime = 0.2f;
        [Header("地上 : 標準移動速度"), SerializeField]
        private float _moveSpeed = 4f;
        [Header("空中水平移動制御")]
        [Tooltip("空中 : 移動加速度"), SerializeField]
        private float _midairMoveAcceleration = 4f;
        [Tooltip("空中 : 移動最大速度"), SerializeField]
        private float _maxMidairMoveSpeed = 4f;
        [Header("減速値")]
        [Tooltip("減速度（地上）"), SerializeField]
        private float _landDeceleration = 20f;
        [Tooltip("減速度（空中）"), SerializeField]
        private float _midairDeceleration = 10f;
        [Header("現在の速度")]
        [Tooltip("現在の速度 : インスペクタで値を追跡する用"), SerializeField]
        private float _currentSpeed = 0f;

        [Header("垂直移動")]
        [Tooltip("ジャンプ力"), SerializeField]
        private float _jumpPower = 4f;

        private float _toMoveTimer = 0f;
        private HorizontalMoveMode _currentHorizontalMoveMode = HorizontalMoveMode.Stop;
        private float _previousDir = 0f;
        private float _moveHorizontalDir = 0f;

        private PlayerController _playerController;

        /// <summary>
        /// 移動できるかどうかを表す値 <br/>
        /// この値が trueであれば、移動入力が発生したときに移動処理を実行する。
        /// </summary>
        public bool CanMove { get; set; } = true;
        /// <summary>
        /// ジャンプできるかどうかを表す値 <br/>
        /// この値が trueであれば、ジャンプ入力が発生したときにジャンプ処理を実行する。
        /// </summary>
        public bool CanJump { get; set; } = true;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void ClearHorizontalSpeed()
        {
            _currentSpeed = 0f;
        }

        public void Update()
        {
            // 入力が発生しているとき、現在速度を加算する。
            if (_playerController.InputManager.IsExist[InputType.MoveHorizontal] && CanMove)
            {
                // 一つの入力方向を保存しておく
                _previousDir = _moveHorizontalDir;
                // 方向を表す値を更新する。（右に相当する値なら1, 左に相当する値なら-1。）
                _moveHorizontalDir = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) > 0f ? 1f : -1f;

                if (Mathf.Abs(_previousDir - _moveHorizontalDir) > 0.1f)
                {
                    _currentSpeed = 0f;
                    _toMoveTimer = 0f;
                    _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                } // 方向転換したら再度低速移動から開始する。

                // 接地しているとき
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    // プレイヤーの状態によって処理を変える。
                    switch (_currentHorizontalMoveMode)
                    {
                        case HorizontalMoveMode.Stop:
                            _toMoveTimer = 0f;
                            _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                            break;
                        case HorizontalMoveMode.Start:
                            _currentSpeed = _startSpeed;
                            _toMoveTimer += Time.deltaTime;
                            if (_toMoveTimer > _toMoveTime) _currentHorizontalMoveMode = HorizontalMoveMode.Move;
                            break;
                        case HorizontalMoveMode.Move:
                            _currentSpeed = _moveSpeed;
                            break;
                        case HorizontalMoveMode.Deceleration:
                            _toMoveTimer = 0f;
                            _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                            break;
                    }
                }
                // 接地していないとき
                else
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Move;

                    _currentSpeed += Time.deltaTime * _midairMoveAcceleration;
                    if (_currentSpeed > _maxMidairMoveSpeed) _currentSpeed = _maxMidairMoveSpeed; // 最大速度を超えない
                }
            }
            // 入力がないとき、現在速度を減算する。
            else
            {
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    _currentSpeed -= Time.deltaTime * _landDeceleration;
                }
                else
                {
                    _currentSpeed -= Time.deltaTime * _midairDeceleration;
                }
                if (0f > _currentSpeed) _currentSpeed = 0f; // 0より小さくならない

                if (_currentSpeed > 0.01f)
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Deceleration;
                }
                else
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Stop;
                }
            }

            if (Mathf.Abs(_currentSpeed) > 0.01f)
            {
                // 横の入力に応じて左右に移動する
                _playerController.Rigidbody2D.velocity =
                        new Vector2(
                            _moveHorizontalDir * _currentSpeed,
                            _playerController.Rigidbody2D.velocity.y);
            }

            if (CanJump)
            {
                // ジャンプ処理
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX) &&
                    _playerController.InputManager.IsPressed[InputType.Jump])
                {
                    _playerController.Rigidbody2D.velocity =
                    new Vector2(
                        _playerController.Rigidbody2D.velocity.x,
                        _jumpPower);
                }
            }
        }
        /// <summary>
        /// プレイヤーの水平方向移動状態を表現する値
        /// </summary>
        private enum HorizontalMoveMode
        {
            /// <summary> 停止状態 </summary>
            Stop,
            /// <summary> 入力発生直後 </summary>
            Start,
            /// <summary> 通常移動 </summary>
            Move,
            /// <summary> 減速 </summary>
            Deceleration,
        }
    }
}