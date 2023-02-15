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
        [Tooltip("標準的な弾を割り当ててください"), SerializeField]
        private StandardBullet _standardBullet = default;

        /// <summary> 標準的な銃の弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _standardBulletCount = new ReactiveProperty<int>();
        /// <summary> 敵を貫通する弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _penetrateBulletCount = new ReactiveProperty<int>();
        /// <summary> 壁を反射する弾の"所持数"を表現する値 </summary>
        private ReactiveProperty<int> _reflectBulletCount = new ReactiveProperty<int>();

        private Dictionary<BulletType, ReactiveProperty<int>> _bullets
            = new Dictionary<BulletType, ReactiveProperty<int>>();

        /// <summary> 標準的な銃の弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> StandardBulletCount => _standardBulletCount;
        /// <summary> 敵を貫通する弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> PenetrateBulletCount => _penetrateBulletCount;
        /// <summary> 壁を反射する弾の"所持数"を表現する値 </summary>
        public IReadOnlyReactiveProperty<int> ReflectBulletCount => _reflectBulletCount;

        /// <summary> このクラスの初期化処理 </summary>
        public void Setup()
        {
            // 各ReactivePropertyをディクショナリに登録する。
            _bullets.Add(BulletType.StandardBullet, _standardBulletCount);
            _bullets.Add(BulletType.PenetrateBullet, _penetrateBulletCount);
            _bullets.Add(BulletType.ReflectBullet, _reflectBulletCount);
        }
        /// <summary> 弾数を設定する </summary>
        /// <param name="type"> 弾の種類 </param>
        /// <param name="setValue"> 設定する値 </param>
        public void SetBullet(BulletType type, int setValue)
        {
            _bullets[type].Value = setValue;
        }
        /// <summary> 弾を装填する </summary>
        /// <param name="type"> 弾の種類 </param>
        /// <returns> 装填する弾があるとき、弾を、そうでないときnullを返す。 </returns>
        public BulletBase LoadBullet(BulletType type)
        {
            // 弾の所持数を確認する。
            if (_bullets[type].Value > 0)
            {
                _bullets[type].Value--; // 弾の所持数を減らす。
                return _standardBullet; // 弾のインスタンスを返す。
            } // 弾がある場合の処理。
            else
            {
                return null;
            } // 弾が無ければ null を返す。
        }
        /// <summary> 弾を拾う処理（指定された種類の弾を一つだけ増やす） </summary>
        /// <param name="type"> 弾の種類 </param>
        public void GetBullet(BulletType type)
        {
            _bullets[type].Value++;
        }
    }
}