// 日本語対応
using HitSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Input;
using Bullet;
using UI;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private UIController _uIController;
        [SerializeField]
        private Move _move = default;
        [SerializeField]
        private OverlapCircle2D _groungChecker = default;
        [SerializeField, HideInInspector]
        private DirectionControl _directionControler = default;
        [SerializeField]
        private RevolverOperator _revolverOperator = default;
        [SerializeField]
        private Revolver _revolver = default;
        [SerializeField]
        private BulletCountManager _bulletCountManager = default;
        [SerializeField]
        private BulletDataBase _bulletDataBase = default;
        [SerializeField]
        private LifeController _lifeController = default;
        [SerializeField]
        private Avoidance _avoidance = default;

        private Rigidbody2D _rigidbody2D = null;

        public UIController UIController => _uIController;
        public Move Move => _move;
        public OverlapCircle2D GroungChecker => _groungChecker;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public InputManager InputManager { get; private set; } = new InputManager();
        public DirectionControl DirectionControler => _directionControler;
        public DeviceManager DeviceManager { get; private set; } = new DeviceManager();
        public RevolverOperator RevolverOperator => _revolverOperator;
        public Revolver Revolver => _revolver;
        public BulletCountManager BulletCountManager => _bulletCountManager;
        public BulletDataBase BulletDataBase => _bulletDataBase;
        public LifeController LifeController => _lifeController;
        public Avoidance Avoidance => _avoidance;


        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            InputManager.Init();
            _move.Init(this);
            _groungChecker.Init(transform);
            _directionControler.Init(transform);
            _revolver.Init(this);
            _bulletCountManager.Setup();
            _revolverOperator.Init(this);
            _bulletDataBase.Init();
            _avoidance.Init(this);
            TestRevolverLoading(); // テスト
        }
        private void Update()
        {
            DeviceManager.Update();       // デバイス制御
            DirectionControler.Update();  // 方向制御
            _move.Update();               // 移動処理
            _revolverOperator.Update();   // リボルバー操作の更新
            _revolver.Update();           // リボルバーの更新処理
            _revolver.OnDrawAimingLine(); // 照準描画処理（カメラの更新タイミングと合わせる必要有り）
            _avoidance.Update();          // 回避制御
        }
        private void OnDrawGizmosSelected()
        {
            _groungChecker.OnDrawGizmos(transform, DirectionControler.MovementDirectionX);
        }

        #region Test
        [Header("リボルバーテスト用")]
        [SerializeField]
        private BulletBase _basicBullet = default;
        /// <summary>
        /// 全てのチェンバーに弾を装填する
        /// </summary>
        private void TestRevolverLoading()
        {
            for (int i = 0; i < _revolver.Cylinder.Length; i++)
            {
                _revolver.LoadBullet(_basicBullet, i);
            }
        }
        #endregion
    }
}