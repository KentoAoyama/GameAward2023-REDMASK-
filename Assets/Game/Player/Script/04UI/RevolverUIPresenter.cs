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

        [Header("各弾の色")]
        [SerializeField]
        private Color _standardBulletImage = default;
        [SerializeField]
        private Color _penetrateBulletImage = default;
        [SerializeField]
        private Color _reflectBulletImage = default;
        [SerializeField]
        private Color _shellCaseImage = default;

        private PlayerController _playerController = null;
        private Dictionary<BulletType, Color> _bulletColors = new Dictionary<BulletType, Color>();

        public void Init(PlayerController playerController)
        {
            _bulletColors.Add(BulletType.StandardBullet, _standardBulletImage);
            _bulletColors.Add(BulletType.PenetrateBullet, _penetrateBulletImage);
            _bulletColors.Add(BulletType.ReflectBullet, _reflectBulletImage);
            _bulletColors.Add(BulletType.ShellCase, _shellCaseImage);
            _bulletColors.Add(BulletType.Empty, Color.clear);

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
                if (_bulletColors.TryGetValue(bulletType, out Color result))
                {
                    _chamber[targetChamberNumber].color = result;
                }
                else
                {
                    Debug.LogWarning("ディクショナリから値の取得に失敗した、、、");
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