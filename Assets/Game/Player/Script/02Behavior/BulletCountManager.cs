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
    public class BulletCountManager
    {
        /// <summary> 標準的な銃の弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _standardBulletCount = new ReactiveProperty<int>();
        /// <summary> 敵を貫通する弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _penetrateBulletCount = new ReactiveProperty<int>();
        /// <summary> 壁を反射する弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _reflectBulletCount = new ReactiveProperty<int>();

        private Dictionary<BulletType, ReactiveProperty<int>> _bulletCounts
            = new Dictionary<BulletType, ReactiveProperty<int>>();

        /// <summary> 標準的な銃の弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> StandardBulletCount => _standardBulletCount;
        /// <summary> 敵を貫通する弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> PenetrateBulletCount => _penetrateBulletCount;
        /// <summary> 壁を反射する弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> ReflectBulletCount => _reflectBulletCount;
        public Dictionary<BulletType, IReadOnlyReactiveProperty<int>> BulletCounts { get; private set; } = new Dictionary<BulletType, IReadOnlyReactiveProperty<int>>();

        /// <summary> このクラスの初期化処理 </summary>
        public void Setup()
        {
            // 各ReactivePropertyをディクショナリに登録する。
            _bulletCounts.Add(BulletType.StandardBullet, _standardBulletCount);
            _bulletCounts.Add(BulletType.PenetrateBullet, _penetrateBulletCount);
            _bulletCounts.Add(BulletType.ReflectBullet, _reflectBulletCount);

            BulletCounts.Add(BulletType.StandardBullet, _standardBulletCount);
            BulletCounts.Add(BulletType.PenetrateBullet, _penetrateBulletCount);
            BulletCounts.Add(BulletType.ReflectBullet, _reflectBulletCount);
        }
        /// <summary> 弾数を設定する </summary>
        /// <param name="type"> 弾の種類 </param>
        /// <param name="setValue"> 設定する値 </param>
        public void SetBullet(BulletType type, int setValue)
        {
            _bulletCounts[type].Value = setValue;
        }
        /// <summary>
        /// 所持数から弾を減らす
        /// </summary>
        public bool ReduceOneBullet(BulletType type)
        {
            if (_bulletCounts[type].Value > 0)
            {
                _bulletCounts[type].Value--;
                return true;
            }
            else
            {
                Debug.LogWarning($"{type} の所持数はゼロです。");
                return false;
            }
        }
        /// <summary> 弾を拾う処理（指定された種類の弾を一つだけ増やす） </summary>
        /// <param name="type"> 弾の種類 </param>
        public void GetBullet(BulletType type)
        {
            _bulletCounts[type].Value++;
        }
    }
}