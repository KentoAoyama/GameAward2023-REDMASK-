using UnityEngine;

namespace Bullet
{
    [System.Serializable]
    public class StandardBullet : BulletBase
    {
        public override BulletType Type => BulletType.StandardBullet;

        protected override void OnHitCollision(Collision2D target)
        {

        }

        protected override void OnHitTrigger(Collider2D target)
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