using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 弾の基底クラス
    /// </summary>
    public abstract class BulletBase : MonoBehaviour, IStoreableInChamber, IPausable
    {
        [Tooltip("何体の敵にヒットできるか"), SerializeField]
        protected int _maxEnemyHitNumber = 1;
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
        [Tooltip("描画するガイドラインの長さ"), SerializeField]
        private float _guidelineLength = 1f;

        protected int _currentEnemyHitNumber = 0;
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
        /// <summary> 描画するガイドラインの長さ </summary>
        public float GuidelineLength => _guidelineLength;

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

        private void OnEnable()
        {
            GameManager.Instance.PauseManager.Register(this);
        }
        private void OnDisable()
        {
            GameManager.Instance.PauseManager.Lift(this);
        }

        private Vector2 _velocity;
        private float _angularVelocity;
        public void Pause()
        {
            _velocity = _rigidbody2D.velocity;
            _angularVelocity = _rigidbody2D.angularVelocity;
            _rigidbody2D.Sleep();
            _rigidbody2D.simulated = false;
        }

        public void Resume()
        {
            _rigidbody2D.simulated = true;
            _rigidbody2D.WakeUp();
            _rigidbody2D.angularVelocity = _angularVelocity;
            _rigidbody2D.velocity = _velocity;
        }
    }
}