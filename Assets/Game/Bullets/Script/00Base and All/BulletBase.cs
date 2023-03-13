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

        protected Rigidbody2D _rigidbody2D = null;
        private Vector2 _shootAngle = default;

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

        public void Setup(Vector2 shootAngle)
        {
            _shootAngle = shootAngle;
        }
        protected virtual void Start()
        {
            // SpriteRendereの初期化処理
            var sr = GetComponent<SpriteRenderer>();
            sr.color = _color;
            sr.sprite = _gameSprite;
            // Rigidbody2Dを取得
            _rigidbody2D = GetComponent<Rigidbody2D>();
            // 指定した方向、速度で弾を移動させる。
            _rigidbody2D.velocity = _shootAngle.normalized * _shootSpeed * GameManager.Instance.TimeController.CurrentTimeScale.Value;
        }

        private void Update()
        {
            // 移動ベクトル       =  方向ベクトル                    *  速度       *  時間の大きさ
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _shootSpeed * GameManager.Instance.TimeController.CurrentTimeScale.Value;
        }
        private void OnBecameInvisible()
        {
            Destroy(this.gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnHitTrigger(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnHitCollision(collision);
        }
        protected abstract void OnHitTrigger(Collider2D target);
        protected abstract void OnHitCollision(Collision2D target);
    }
}