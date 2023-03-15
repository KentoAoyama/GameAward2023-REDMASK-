using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 反射弾クラス
    /// </summary>
    [System.Serializable]
    public class ReflectBullet : BulletBase
    {
        [Tooltip("最大何回壁を反射するか"), SerializeField]
        private int _maxWallCollisionCount = 0;
        [TagName, SerializeField]
        private string _wallTag = default;
        [SerializeField]
        private LayerMask _wallLayer = default;

        private CircleCollider2D _circleCollider2D = null;
        /// <summary> 現在の反射回数 </summary>
        private int _reflexCount = 0;

        public override BulletType Type => BulletType.ReflectBullet;
        public int MaxWallCollisionCount => _maxWallCollisionCount;

        protected override void Start()
        {
            base.Start();
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        protected override void OnHitCollision(Collision2D target)
        {

        }
        protected override void OnHitTrigger(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(_attackPower);
            }
            if (target.tag == _wallTag)
            {
                _reflexCount++;

                if (_reflexCount >= _maxWallCollisionCount)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    // 自分の位置に、自分より一回り大きいサークルキャストを発生させる。
                    var hit = Physics2D.CircleCast(this.transform.position,
                        _circleCollider2D.radius * transform.localScale.x ,
                        Vector2.zero, 0f, _wallLayer);
                    if (hit.collider == null)
                    {
                        Debug.LogWarning("レイがコライダーにヒットしませんでした。");
                        return;
                    }
                    // 移動方向ベクトルを反射する。
                    _rigidbody2D.velocity = Vector2.Reflect(_rigidbody2D.velocity, hit.normal);

                }
            }
        }
    }
}