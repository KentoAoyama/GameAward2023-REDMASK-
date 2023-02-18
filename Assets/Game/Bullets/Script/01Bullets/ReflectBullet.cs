using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 反射弾クラス
    /// </summary>
    [System.Serializable]
    public class ReflectBullet : BulletBase
    {
        [TagName, SerializeField]
        private string _wallTag = default;

        public override BulletType Type => BulletType.ReflectBullet;

        private Vector2 _velocity = default;

        public void Update()
        {
            _velocity = _rigidbody2D.velocity;
        }

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