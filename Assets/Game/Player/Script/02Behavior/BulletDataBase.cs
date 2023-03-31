using UnityEngine;
using Bullet;
using System;
using System.Collections.Generic;

namespace Player
{
    /// <summary>
    /// 弾のデータを管理、提供するクラス
    /// </summary>
    [Serializable]
    public class BulletDataBase
    {
        [SerializeField]
        private StandardBullet2 _standardBullet = null;
        [SerializeField]
        private PenetrateBullet2 _penetrateBullet = null;
        [SerializeField]
        private ReflectBullet2 _reflectBullet = null;

        public Dictionary<BulletType, Bullet2> Bullets { get; private set; } = new Dictionary<BulletType, Bullet2>();

        public bool IsInit { get; private set; } = false; 

        public void Init()
        {
            Bullets.Add(BulletType.StandardBullet, _standardBullet);
            Bullets.Add(BulletType.PenetrateBullet, _penetrateBullet);
            Bullets.Add(BulletType.ReflectBullet, _reflectBullet);
            IsInit = true;
        }
        /// <summary>
        /// シリアライズフィールドで割り当てない場合このメソッドを呼び出す
        /// </summary>
        public void LoadData()
        {

        }
    }
}
