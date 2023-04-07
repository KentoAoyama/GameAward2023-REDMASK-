using System;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class ReflectBullet2 : Bullet2
    {
        [Tooltip("壁を反射する回数"), SerializeField]
        private int _maxWallCollisionCount = 0;

        public int MaxWallCollisionCount => _maxWallCollisionCount;

        /// <summary> 反射弾は盾を貫通しない </summary>
        protected override bool IsPenetrateShield => false;
    }
}