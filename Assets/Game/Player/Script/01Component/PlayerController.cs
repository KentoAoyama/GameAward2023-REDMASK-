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
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPausable, IDamageable
    {

        [SerializeField]
        public GameObject _camerad;

        [Header("Test用。GameOverのUI")]
        [Tooltip("Test用。GameOverのUI"), SerializeField] private GameObject _gameOverUI;

        [Header("プレイヤーのオブジェクト")]
        [Tooltip("プレイヤーのオブジェクト"), SerializeField]
        private GameObject _player;



        [Header("プレイヤーのアニメーター")]
        [Tooltip("プレイヤーのアニメーター"), SerializeField]
        private Animator _playerAnim;

        [Header("プレイヤーについているシネマシーン")]
        [Tooltip("プレイヤーのアニメーター"), SerializeField]
        private CinemachineVirtualCamera _camera;

        [Header("演出用のテキスト"), SerializeField]
        private Text _performanceText = default;


        [Tooltip("プレイヤーのUIを管理するクラス"), SerializeField]
        private UIController _uIController;

        [Tooltip("アニメーター制御"), SerializeField]
        private PlayerAnimationControl _playerAnimatorControl = default;
        [Tooltip("腕のアニメーション"), SerializeField]
        private PlayerBodyAndArmAngleSetting _bodyAngleSetting = default;
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

        [Tooltip("攻撃当たり判定"), SerializeField]
        private OverlapBox2D _proximityHitChecker = default;
        [Tooltip("近接攻撃"), SerializeField]
        private Proximity _proximity = default;
        [Tooltip("カメラ制御"), SerializeField]
        private CameraShake _camraControl = default;
        [Tooltip("構え"), SerializeField]
        private GunSetUp _gunSetUp = default;


        private Rigidbody2D _rigidbody2D = null;

        /// <summary>死亡しているかどうかを示す</summary>
        private bool _isDead = false;

        public bool IsDead => _isDead;
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

        public GunSetUp GunSetUp => _gunSetUp;
        public PlayerBodyAndArmAngleSetting BodyAnglSetteing => _bodyAngleSetting;

        public bool IsSetUp { get; private set; } = false;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            InputManager.Init();
            _move.Init(this);
            _groungChecker.Init(transform);
            _directionControler.Init(transform);
            _revolver.Init(this);
            _bulletCountManager.Setup(this);
            _revolverOperator.Init(this);
            _bulletDataBase.Init();
            _avoidance.Init(this);
            TestRevolverLoading(); // テスト

            _playerAnimatorControl.Init(this);
            _proximity.Init(this);
            _proximityHitChecker.Init(transform);
            _camraControl.Init(this);
            _bodyAngleSetting.Init(this);
            _gunSetUp.Init(this);

            IsSetUp = true;

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

                _gunSetUp.UpData();

                //_camraControl.CameraShakeSpeed(); //カメラの再生速度

                _bodyAngleSetting.Update();

                _playerAnimatorControl.SetAnimatorParameters();
            }
        }
        private void OnDrawGizmosSelected()
        {
            _groungChecker.OnDrawGizmos(transform, DirectionControler.MovementDirectionX);
            _proximityHitChecker.OnDrawGizmos(transform, DirectionControler.MovementDirectionX);
        }


        /// <summary>ゲームクリア時に呼ぶ、ホームに弾を追加</summary>
        public void StageComplete()
        {
            // ホームに弾を返す。
            _bulletCountManager.HomeBulletAddEndStage();
            // GameManagerが持つステージ用弾数,シリンダーの状態をリセット。
            GameManager.Instance.BulletsCountManager.Clear();
            // GameManager.BulletsCountManagerの情報を保存。
            GameManager.Instance.BulletsCountManager.Save();
        }


        [Header("Test用に、Sceneで設定したものを使うならTrueにしてね")]
        [SerializeField]
        private bool _isTestRevoler = true;

        [Header("リボルバーテスト用")]
        [SerializeField]
        private Bullet2 _basicBullet = default;

        /// <summary>
        /// 全てのチェンバーに弾を装填する
        /// </summary>
        private async void TestRevolverLoading()
        {
            if (_isTestRevoler)
            {
                for (int i = 0; i < _revolver.Cylinder.Length; i++)
                {
                    _revolver.LoadBullet(_basicBullet, i);
                }
            } //Test用に、Sceneで設定したものを使う
            else
            {
                _revolver.SetCylinderIndex(GameManager.Instance.StageManager.CylinderIndex);
                // 弾の情報が読み込まれるまで待機
                await UniTask.WaitUntil(() => BulletDataBase.IsInit);

                // プレイヤー死亡時 直前からやり直すを選択した場合の処理
                // あるいは、面から面へ遷移する際。
                if (GameManager.Instance.StageManager.StageStartMode == StageStartMode.JustBefore &&
                    GameManager.Instance.StageManager.CheckPointCylinder != null)
                {
                    for (int i = 0; i < GameManager.Instance.StageManager.CheckPointCylinder.Length; i++)
                    {
                        if (GameManager.Instance.StageManager.CheckPointCylinder[i] == null)
                        {
                            _revolver.LoadBullet(null, i);
                        }
                        else
                        {
                            BulletDataBase.Bullets.TryGetValue(GameManager.Instance.StageManager.CheckPointCylinder[i].Type, out Bullet2 bullet);
                            _revolver.LoadBullet(bullet, i);
                        }
                    }
                }
                else
                {
                    // プレイヤー死亡時 最初からやり直すを選択した場合の処理
                    // あるいは、準備シーンからステージへ遷移する際。
                    for (int i = 0; i < _revolver.Cylinder.Length; i++)
                    {
                        BulletDataBase.Bullets.TryGetValue(GameManager.Instance.BulletsCountManager.Cylinder[i], out Bullet2 bullet);
                        _revolver.LoadBullet(bullet, i);
                    }
                }
            }　//アジトで設定したものを使う
        }
        private IStoreableInChamber BulletTypeTofIStoreableInChamber(BulletType bulletType)
        {
            switch (bulletType)
            {
                case BulletType.StandardBullet: return _bulletDataBase.Bullets[bulletType];
                case BulletType.PenetrateBullet: return _bulletDataBase.Bullets[bulletType];
                case BulletType.ReflectBullet: return _bulletDataBase.Bullets[bulletType];
                default: return default;
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

            //回避モーションの一時停止
            _avoidance.Pause();

            //カメラの振動一時停止
            _camraControl.Pause();
        }

        public void Resume()
        {
            _move.Resume();
            InputManager.InputActionCollection.Enable();
            _revolver.IsPause = false;

            //回避モーションの再開
            _avoidance.Resume();

            //カメラの振動の再開
            _camraControl.Pause();
        }

        /// <summary>ダメージを受けた時の処理</summary>
        public void Damage()
        {
            if (_lifeController.IsGodMode) return;

            //死体撃ちで、2回呼ばれないようにする
            if (!_isDead)
            {
                //音を鳴らす
                GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Damage");

                //重力を戻す
                _rigidbody2D.gravityScale = 1f;

                //死亡した
                _isDead = true;

                //GameOverのUIを出す
                _gameOverUI.SetActive(true);

                //近接攻撃中に当たったら、近接攻撃を強制終了させる
                _proximity.AttackEnd();

                //死亡時のカメラの揺れ
                _camraControl.DeadCameraShake();

                //時が遅くなっているのを解除
                GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);
            }
        }

        public void TestRetry()
        {
            _isDead = false;

            //GameOverのUIを消す
            _gameOverUI.SetActive(false);
        }

        // 以下はタイムラインから呼び出す事を想定して作成したメソッド群
        public void InputDisable()
        {
            // Debug.Log("入力停止");
            InputManager.InputActionCollection.Disable();
            _performanceText.text = "開始";
        }
        public void InputEnable()
        {
            // Debug.Log("入力復帰");
            InputManager.InputActionCollection.Enable();
            _performanceText.text = "会話中";
        }
        public void WaitInputText()
        {
            _performanceText.text = "発砲ボタンを押下してください";
        }
        public void OnFireTimeline()
        {
            _performanceText.text = "終了";
        }
        public void TestPerformanceTextClear()
        {
            _performanceText.text = "";
        }
    }
}