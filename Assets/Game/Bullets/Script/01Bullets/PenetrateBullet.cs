using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 貫通弾クラス
    /// </summary>
    [System.Serializable]
    public class PenetrateBullet : BulletBase
    {
        public override BulletType Type => BulletType.PenetrateBullet;
        protected override void OnHit(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable hit))
            {
                hit.Damage(_attackPower);
            }
            // 自身を破棄する
            Destroy(this.gameObject);
        }
    }
}