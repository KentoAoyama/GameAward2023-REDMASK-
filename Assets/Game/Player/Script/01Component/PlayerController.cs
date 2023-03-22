// 日本語対応
using HitSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Input;
using Bullet;
using UI;
using Cysharp.Threading.Tasks;
using Cinemachine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
<<<<<<< HEAD
    public class PlayerController : MonoBehaviour, IPausable, IDamageable
=======
    public class PlayerController : MonoBehaviour, IPausable,IDamageable
>>>>>>> 86e333fb58cbaf1e3887bf1c72518885847fb10e
    {
        [Header("プレイヤーのオブジェクト")]
        [Tooltip("プレイヤーのオブジェクト"), SerializeField]
        private GameObject _player;

        [Header("プレイヤーのアニメーター")]
        [Tooltip("プレイヤーのアニメーター"), SerializeField]
        private Animator _playerAnim;

        [Header("プレイヤーについているシネマシーン")]
        [Tooltip("プレイヤーのアニメーター"), SerializeField]
        private CinemachineVirtualCamera _camera;

        [Tooltip("プレイヤーのUIを管理するクラス"), SerializeField]
        private UIController _uIController;
        [Tooltip("移動制御"), SerializeField]
        private Move _move = default;
        [Tooltip("接地判定"), SerializeField]
        private OverlapCircle2D _groungChecker = default;
        [Tooltip("移動方向、"), SerializeField, HideInInspector]
        private DirectionControl _directionControler = default;
        [Tooltip("リボルバー制御"), SerializeField]
        private RevolverOperator _revolverOperator = default;
        [Tooltip("リボルバー"), SerializeField]
        private Revolver _revolver = default;
        [Tooltip("弾の所持数制御"), SerializeField]
        private BulletCountManager _bulletCountManager = default;
        [Tooltip("弾の情報を保持,提供するクラス"), SerializeField]
        private BulletDataBase _bulletDataBase = default;
        [Tooltip("ライフ制御"), SerializeField]
        private LifeController _lifeController = default;
        [Tooltip("回避制御"), SerializeField]
        private Avoidance _avoidance = default;
        [Tooltip("アニメーター制御"), SerializeField]
        private PlayerAnimationControl _playerAnimatorControl = default;
        [Tooltip("攻撃当たり判定"), SerializeField]
        private OverlapBox2D _proximityHitChecker = default;
        [Tooltip("近接攻撃"), SerializeField]
        private Proximity _proximity = default;
        [Tooltip("カメラ制御"), SerializeField]
        private CameraShake _camraControl = default;

        private Rigidbody2D _rigidbody2D = null;

        [Tooltip("プレイヤーが死亡")]
        private bool _isDead = false;


        public GameObject Player => _player;

        public CinemachineVirtualCamera Camera => _camera;
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
        public PlayerAnimationControl PlayerAnimatorControl => _playerAnimatorControl;
        public OverlapBox2D ProximityHitChecker => _proximityHitChecker;
        public Animator PlayerAnim => _playerAnim;
        public Proximity Proximity => _proximity;

        public CameraShake CameraControl => _camraControl;

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

            _playerAnimatorControl.Init(this);
            _proximity.Init(this);
            _proximityHitChecker.Init(transform);
            _camraControl.Init(this);
        }
        private void Update()
        {
            if (!_isDead)
            {

                DeviceManager.Update();       // デバイス制御
                DirectionControler.Update();  // 方向制御

                _move.Update();               // 移動処理
                _revolverOperator.Update();   // リボルバー操作の更新
                _revolver.Update();           // リボルバーの更新処理
                _revolver.OnDrawAimingLine(); // 照準描画処理（カメラの更新タイミングと合わせる必要有り）
                _avoidance.Update();          // 回避制御
                _proximity.Update();          //近接攻撃
            }
        }
        private void OnDrawGizmosSelected()
        {
            _groungChecker.OnDrawGizmos(transform, DirectionControler.MovementDirectionX);
            _proximityHitChecker.OnDrawGizmos(transform, DirectionControler.MovementDirectionX);
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

        private void OnEnable()
        {
            GameManager.Instance.PauseManager.Register(this);


        }
        private void OnDisable()
        {
            GameManager.Instance.PauseManager.Lift(this);
        }

        public async void Pause()
        {
            // 物理演算の停止、入力の停止、
            _move.Pause();
            await UniTask.WaitUntil(() => InputManager.InputActionCollection != null);
            InputManager.InputActionCollection.Disable();
            _revolver.IsPause = true;

            _avoidance.Pause();
        }

        public void Resume()
        {
            _move.Resume();
            InputManager.InputActionCollection.Enable();
            _revolver.IsPause = false;
            _avoidance.Resume();
        }

<<<<<<< HEAD
        public void Damage()
=======
        public void Damage(float value)
>>>>>>> 86e333fb58cbaf1e3887bf1c72518885847fb10e
        {

        }
        #endregion
    }
}