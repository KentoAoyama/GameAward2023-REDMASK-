using System;
using UnityEngine;

namespace Bullet
{
    [Serializable]
    public class ReflectBullet2 : Bullet2
    {
        [Tooltip("最大何回壁を反射するか"), SerializeField]
        private int _maxWallCollisionCount = 0;

        public int MaxWallCollisionCount => _maxWallCollisionCount;
    }
}