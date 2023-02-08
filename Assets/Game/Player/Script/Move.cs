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

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Update()
        {
            // 横の入力に応じて左右に移動する
            _playerController.Rigidbody2D.velocity =
                new Vector2(
                    _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) * _moveSpeed,
                    _playerController.Rigidbody2D.velocity.y);

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