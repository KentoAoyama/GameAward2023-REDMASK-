using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Move
    {
        [Header("水平移動系")]
        [Tooltip("加速度"), SerializeField]
        private float _acceleration = 1f;
        [Tooltip("減速度"), SerializeField]
        private float _deceleration = 1f;
        [Tooltip("最大速度"), SerializeField]
        private float _maxSpeed = 4f;
        [Tooltip("現在の速度 : インスペクタで値を追跡する用")]
        private float _currentSpeed = 0f;
        [Header("垂直移動系")]
        [Tooltip("ジャンプ力"), SerializeField]
        private float _jumpPower = 4f;

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

        public void Update()
        {
            // 入力が発生しているとき、現在速度を加算する。
            if (_playerController.InputManager.IsExist[InputType.MoveHorizontal] && CanMove)
            {
                _currentSpeed += Time.deltaTime * _acceleration;
                if (_currentSpeed > _maxSpeed) _currentSpeed = _maxSpeed; // 最大速度より大きくならない

                // 方向を表す値を更新する。（右に相当する値なら1, 左に相当する値なら-1。）
                _moveHorizontalDir = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) > 0f ? 1f : -1f;
            }
            // 入力がないとき、現在速度を加算する。
            else
            {
                _currentSpeed -= Time.deltaTime * _deceleration;
                if (0f > _currentSpeed) _currentSpeed = 0f;
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
    }
}