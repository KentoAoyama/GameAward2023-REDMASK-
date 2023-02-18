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