// 日本語対応
using Bullet;
using Player;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;

namespace UI
{
    [System.Serializable]
    public class RevolverUIPresenter
    {
        [Header("シリンダーとチャンバー")]
        [SerializeField]
        private Image _cylinder = default;
        [SerializeField]
        private Image[] _chamber = default;

        private PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;

            // 発砲時シリンダーをアニメーションさせる
            _playerController.Revolver.OnFire += StartCylinderAnimation;
            _playerController.Revolver.OnChamberStateChanged += ChangeChamberState;
        }

        /// <summary> シリンダーアニメーション </summary>
        /// <param name="nextChamberNumber"> 次のチェンバーの位置 </param>
        private void StartCylinderAnimation(int nextChamberNumber)
        {
            _cylinder.transform.DOLocalRotate(new Vector3(0f, 0f, (float)(nextChamberNumber * 60)), 0.2f);

        }
        /// <summary> チェンバーの状態切り替え処理</summary>
        /// <param name="targetChamberNumber"> 変更されたチャンバーの位置 </param>
        /// <param name="bulletType"> 変更後のバレットの種類 </param>
        private void ChangeChamberState(int targetChamberNumber, BulletType bulletType)
        {
            try
            {
                if (bulletType == BulletType.ShellCase)
                {
                    _chamber[targetChamberNumber].color = Color.yellow;
                }
                else if (_playerController.BulletDataBase.Bullets.TryGetValue(bulletType, out BulletBase result))
                {
                    _chamber[targetChamberNumber].color = result.Color;
                }
                else
                {
                    _chamber[targetChamberNumber].color = Color.clear;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("範囲外が指定されたよ");
                Debug.LogError(e.Message);
            }
        }
    }
}