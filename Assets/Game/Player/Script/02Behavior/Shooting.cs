// 日本語対応
using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 射撃の基準となるクラス
    /// </summary>
    [System.Serializable]
    public class Shooting
    {
        [Tooltip("射出する弾"), SerializeField]
        private GameObject _bullet = default;
        [Tooltip("発射孔"), SerializeField]
        private Transform _muzzle = default;
        [Tooltip("弾の非接触対象"), SerializeField]
        private Collider2D[] _nonCollisionTarget = default;

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        public void Shoot(Vector2 shootAngle)
        {
            // 弾を生成し、弾のセットアップを行う

            if (GameObject.Instantiate(_bullet, _muzzle.position, Quaternion.identity).
                TryGetComponent(out BulletControllerBase bc))
            {
                bc.Setup(shootAngle, _nonCollisionTarget);
            }
        }
    }
}