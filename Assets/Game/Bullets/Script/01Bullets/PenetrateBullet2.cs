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
        [SerializeField]
        private int _maxWallHitNumber = 1;

        public int MaxWallHitNumber => _maxWallHitNumber;
    }
}