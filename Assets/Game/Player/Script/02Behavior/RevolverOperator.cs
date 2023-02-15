// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// リボルバー操作クラス
    /// </summary>
    public class RevolverOperator
    {
        public PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            // 撃てる状態かつ、撃つ入力が発生したとき 銃を撃つ
            if (_playerController.InputManager.GetValue<float>(InputType.Fire1) > 0.49f &&
                _playerController.Revolver.CanFire)
            {
                _playerController.Revolver.Fire();
            }
        }
    }
}