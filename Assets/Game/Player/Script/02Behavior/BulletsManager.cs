using System;
using UnityEngine;
using Bullet;
using System.Collections.Generic;
using UniRx;

namespace Player
{
    /// <summary>
    /// プレイヤーが所持する弾を管理するクラス。
    /// </summary>
    [System.Serializable]
    public class BulletsManager
    {
        /// <summary> 標準的な銃の弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _standardBulletCount = new ReactiveProperty<int>();

        public IReadOnlyReactiveProperty<int> StandardBulletCount => _standardBulletCount;
    }
}