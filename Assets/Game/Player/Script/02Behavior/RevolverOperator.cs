
using Bullet;
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
            // リロード処理
            if (_playerController.InputManager.IsPressed[InputType.LoadBullet])
            {
                // 排莢する
                var cylinder = _playerController.Revolver.EjectShellsAll();
                // 空いているチャンバーを検索する。
                var index = FindEmptyChamber();
                if (index != -1) // 空いているチャンバーが見つかった場合の処理
                {
                    // UIで現在選択している弾を装填する
                    if (_playerController.BulletDataBase.Bullets.TryGetValue(
                            _playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType,
                            out BulletBase bullet))
                    {
                        // 弾を減らす。弾を減らせなかった場合、処理しない。
                        if (_playerController.BulletCountManager.ReduceOneBullet(_playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType))
                        {
                            _playerController.Revolver.Cylinder[index] = bullet;
                            _playerController.Revolver.OnChamberStateChanged
                                (index, _playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType);
                        }
                    }
                    else // 弾の取得に失敗した場合の処理
                    {
                        Debug.LogError(
                            $"装填に失敗しました。\n" +
                            $"_playerController.BulletDataBase.Bulletsに" +
                            $"{_playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType}" +
                            $"は登録されていません！修正してください！");
                    }
                }
            }
        }
        /// <summary> 空のチャンバーを見つける </summary>
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