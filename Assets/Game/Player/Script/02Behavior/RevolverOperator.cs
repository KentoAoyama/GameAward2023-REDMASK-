// 日本語対応
using Bullet;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// リボルバー操作クラス
    /// </summary>
    [System.Serializable]
    public class RevolverOperator
    {
        [SerializeField]
        private UIController _uiController = default;

        private PlayerController _playerController = null;

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
            if (_playerController.InputManager.IsPressed[InputType.LoadBullet])
            {
                // 全ての殻薬莢を排出する
                var cylinder = _playerController.Revolver.EjectShellsAll();
                // 空きのチャンバーに現在選択中の弾を装填する
                var index = FindEmptyChamber();
                if (index != -1)
                {
                    if (_playerController.BulletsManager.Bullets.TryGetValue(
                        _uiController.BulletSelectUIPresenter.CurrentSelectBulletType, out BulletBase bullet))
                    {
                        _playerController.Revolver.Cylinder[index] = bullet;
                        _playerController.Revolver.OnChamberStateChanged(index, _uiController.BulletSelectUIPresenter.CurrentSelectBulletType);
                    }
                }
            }
        }
        /// <summary>
        /// 空のチャンバーを見つける
        /// </summary>
        /// <returns> 空のチャンバーの位置。無い場合 -1を返す。 </returns>
        private int FindEmptyChamber()
        {
            int result = _playerController.Revolver.CurrentChamber;

            for (int i = 0; i < 6; i++)
            {
                if (_playerController.Revolver.
                    Cylinder[(result + i) % _playerController.Revolver.Cylinder.Length] == null)
                {
                    return (result + i) % _playerController.Revolver.Cylinder.Length;
                }
            }

            return -1;
        }
    }
}