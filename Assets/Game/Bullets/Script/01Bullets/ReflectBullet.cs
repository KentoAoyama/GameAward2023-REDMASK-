using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 反射弾クラス
    /// </summary>
    [System.Serializable]
    public class ReflectBullet : BulletBase
    {
        public override BulletType Type => BulletType.ReflectBullet;

        protected override void OnHitCollision(Collision2D target)
        {
            // ダメージを加える
            if (target.collider.TryGetComponent(out IDamageable hit))
            {
                hit.Damage(_attackPower);
                // 自身を破棄する
                Destroy(this.gameObject);
            }
        }

        protected override void OnHitTrigger(Collider2D target)
        {

        }
    }
}