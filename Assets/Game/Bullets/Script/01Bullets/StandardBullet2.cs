using System;

namespace Bullet
{
    [Serializable]
    public class StandardBullet2 : Bullet2
    {
        /// <summary> 通常弾は盾を貫通しない </summary>
        protected override bool IsPenetrateShield => false;
    }
}