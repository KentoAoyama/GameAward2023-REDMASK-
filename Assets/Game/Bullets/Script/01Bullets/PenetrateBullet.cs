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

        protected override void OnHitCollision(Collision2D target)
        {

        }

        protected override void OnHitTrigger(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable hit))
            {
                hit.Damage(_attackPower);
                return;
            }

            Destroy(this.gameObject);
        }
    }
}