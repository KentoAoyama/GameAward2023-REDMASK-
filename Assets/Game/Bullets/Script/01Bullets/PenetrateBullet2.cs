using UnityEngine;
using System;

namespace Bullet
{
    /// <summary>
    /// 貫通弾クラス
    /// </summary>
    [Serializable]
    public class PenetrateBullet2 : Bullet2
    {
        [Tooltip("壁を貫通する回数"), SerializeField]
        private int _maxWallHitNumber = 1;

        public int MaxWallHitNumber => _maxWallHitNumber;

        /// <summary> 貫通弾は盾を貫通する </summary>
        protected override bool IsPenetrateShield => true;
    }
}