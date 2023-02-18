using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 弾の基底クラス
    /// </summary>
    public abstract class BulletBase : MonoBehaviour, IStoreableInChamber
    {
        [Tooltip("この弾の生存時間"), SerializeField]
        protected float _lifeTime = 3f;
        [Tooltip("この弾の発射速度"), SerializeField]
        protected float _shootSpeed = 8f;
        [Tooltip("この弾の威力"), SerializeField]
        protected float _attackPower = 1f;
        [Tooltip("ゲーム内用スプライト"), SerializeField]
        private Sprite _gameSprite = default;
        [Tooltip("弾選択UI用スプライト"), SerializeField]
        private Sprite _bulletSelectUISprite = default;
        [Tooltip("シリンダーUI用スプライト"), SerializeField]
        private Sprite _cylinderUISprite = default;
        [Tooltip("本物の弾のスプライトが届くまで、色で区別するためこの値を使用して区別･管理する。"), SerializeField]
        private Color _color = default;

        private Rigidbody2D _rigidbody2D = null;
        private Vector2 _shootAngle = default;
        private Collider2D[] _nonCollisionTarget = null;

        /// <summary> この弾の種類を表現するプロパティ </summary>
        public abstract BulletType Type { get; }
        /// <summary> ゲーム内用スプライト </summary>
        public Sprite GameSprite => _gameSprite;
        /// <summary> 弾選択UI用スプライト </summary>
        public Sprite BulletSelectUISprite => _bulletSelectUISprite;
        /// <summary> シリンダーUI用スプライト </summary>
        public Sprite CylinderUISprite => _cylinderUISprite;
        /// <summary> 本物の弾のスプライトが届くまで、色で区別するためこの値を使用して区別･管理する。 </summary>
        public Color Color => _color;

        public void Setup(Vector2 shootAngle, Collider2D[] nonCollisionTarget)
        {
            _shootAngle = shootAngle;
            _nonCollisionTarget = nonCollisionTarget;
        }
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            // 指定した方向、速度で弾を移動させる。
            _rigidbody2D.velocity = _shootAngle.normalized * _shootSpeed;
            // 指定した時間経過したら、自身を破棄する。
            Destroy(this.gameObject, _lifeTime);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (var e in _nonCollisionTarget)
            {
                if (e == collision) return;
            } // 非接触対象は無視する

            // 接触時処理
            OnHit(collision);
        }
        /// <summary>
        /// 接触時の処理<br/>
        /// この関数内で、敵に当たった時の処理、壁に接触した時の処理、等を記述してください。
        /// </summary>
        /// <param name="target"> 接触相手 </param>
        protected abstract void OnHit(Collider2D target);
    }
}