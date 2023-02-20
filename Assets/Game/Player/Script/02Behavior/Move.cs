using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Move
    {
        [SerializeField]
        private float _moveSpeed = 1f;
        [SerializeField]
        private float _jumpPower = 4f;

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
            if (CanMove)
            {
                // 横の入力に応じて左右に移動する
                _playerController.Rigidbody2D.velocity =
                    new Vector2(
                        _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) * _moveSpeed,
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