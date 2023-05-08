using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 近接、遠距離、ドローン用
/// 各振る舞いのクラスのメソッドを組み合わせて行動を制御するクラス
/// </summary>
[RequireComponent(typeof(SightSensor))]
[RequireComponent(typeof(MoveBehavior))]
[RequireComponent(typeof(AttackBehavior))]
[RequireComponent(typeof(PerformanceBehavior))]
public class EnemyController : MonoBehaviour, IPausable, IDamageable
{
    private static string AnimationSpeedParam = "Speed";
    private static string DefeatedTransitionLayerName = "DeathEnemy";

    [Header("シーン上に配置されているプレイヤーのタグ")]
    [SerializeField, TagName] private string _playerTagName;
    [Header("敵の各種パラメーターを設定したSO")]
    [Tooltip("各振る舞いのクラスはこのSO内の値を参照して機能する")]
    [SerializeField] protected EnemyParamsSO _enemyParamsSO;
    [Header("左向きに配置する")]
    [Tooltip("既存のオブジェクトを反転させる場合は子のEditorViewオブジェクトも反転させると見た目が合う")]
    [SerializeField] private bool _placedFacingLeft;
    [Header("プレイヤー未発見時は常にIdle状態にする")]
    [SerializeField] private bool _idleWhenUndiscover;

    [Header("デバッグ用:現在の状態を表示するText")]
    [SerializeField] private Text _text;

    protected ReactiveProperty<StateTypeBase> _currentState = new();
    protected StateRegister _stateRegister = new();
    protected MoveBehavior _moveBehavior;
    private EnemyAudioModule _audioModule = new();
    private Transform _player;
    private SightSensor _sightSensor;
    private AttackBehavior _attackBehavior;
    private PerformanceBehavior _performanceBehavior;
    private Animator _animator;
    /// <summary>
    /// Pause()が呼ばれるとtrueにResume()が呼ばれるとfalseになる
    /// </summary>
    private bool _isPause;
    /// <summary>
    /// 撃破された際にtrueになりDefeated状態に遷移する
    /// </summary>
    private bool _isDefeated;

    public EnemyParamsSO Params => _enemyParamsSO;
    public bool IsDefeated => _isDefeated;
    public bool IdleWhenUndiscover => _idleWhenUndiscover;

    private void Awake()
    {
        _sightSensor = GetComponent<SightSensor>();
        _moveBehavior = GetComponent<MoveBehavior>();
        _attackBehavior = GetComponent<AttackBehavior>();
        _performanceBehavior = GetComponent<PerformanceBehavior>();
        _animator = GetComponentInChildren<Animator>();

        InitOnAwake();
    }

    private void Start()
    {
        InitOnStart();
    }

    private void OnDisable()
    {
        _currentState.Value.OnDisable();
    }

    protected virtual void InitOnAwake()
    {
        _stateRegister.Register(StateType.Idle, this);
        _stateRegister.Register(StateType.Search, this);
        _stateRegister.Register(StateType.Discover, this);
        _stateRegister.Register(StateType.Move, this);
        _stateRegister.Register(StateType.Attack, this);
        _stateRegister.Register(StateType.Defeated, this);
        _currentState.Value = _stateRegister.GetState(StateType.Idle);
    }

    private void InitOnStart()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
        if (_placedFacingLeft) _moveBehavior.TurnLeft();

        GameManager.Instance.PauseManager.Register(this);
        this.OnDisableAsObservable().Subscribe(_ => GameManager.Instance.PauseManager.Lift(this));

        this.UpdateAsObservable().Where(_ => !_isPause).Subscribe(_ =>
        {
            _currentState.Value = _currentState.Value.Execute();
            _animator.SetFloat(AnimationSpeedParam, GameManager.Instance.TimeController.EnemyTime);
        });

        this.UpdateAsObservable().Where(_ => _text != null).Subscribe(_ =>
        {
            _text.text = _currentState.Value.ToString();
        });
    }

    /// <summary>
    /// その場で待機する。IdleとAttack状態の時、毎フレーム呼ばれる
    /// </summary>
    public void UpdateIdle() => _moveBehavior.Idle();

    /// <summary>
    /// 現在の移動をキャンセルしてプレイヤーに向けて移動する
    /// Move状態での移動をする際にステートのEnter()で呼ばれる
    /// </summary>
    public void MoveToPlayer()
    {
        _moveBehavior.CancelMoveToTarget();
        _moveBehavior.StartMoveToTarget(_player, Params.RunSpeed);
    }

    /// <summary>
    /// 現在の移動をキャンセルして周囲のランダムな個所に移動する
    /// Search状態での移動をする際にステートのEnter()で呼ばれる
    /// </summary>
    public void MoveSeachForPlayer()
    {
        _moveBehavior.CancelMoveToTarget();
        _moveBehavior.StartMoveSearchForPlayer(Params.RunSpeed, Params.TurningPoint, Params.UseRandomTurningPoint);
    }

    /// <summary>
    /// 遷移する際に現在の移動をキャンセルする場合にステートから呼ばれる
    /// </summary>
    public void CancelMoveToTarget() => _moveBehavior.CancelMoveToTarget();

    /// <summary>
    /// 視界に対してプレイヤーがどの位置にいるのかを判定する
    /// 各ステートの実行中呼ばれ続ける
    /// </summary>
    public SightResult LookForPlayerInSight() => _sightSensor.LookForPlayerInSight(Params.SightRadius, 
        Params.SightAngle, Params.AttackRange, Params.IsIgnoreObstacle);

    /// <summary>
    /// Attack状態の時、一定間隔で呼ばれる
    /// </summary>
    public virtual void Attack() => _attackBehavior.Attack();

    /// <summary>
    /// 各ステートから再生するアニメーションを呼び出す
    /// </summary>
    public void PlayAnimation(AnimationName name) => _animator.Play(Params.GetAnimationHash(name), 0, 0);

    /// <summary>
    /// 発見時の演出を行う
    /// </summary>
    public void PlayDiscoverPerformance() => _performanceBehavior.Discover();

    /// <summary>
    /// 各ステートはこのメソッドを呼ぶことで遷移先のステートを取得する
    /// </summary>
    public StateTypeBase GetState(StateType type) => _stateRegister.GetState(type);

    public void Pause()
    {
        _isPause = true;
        _currentState.Value.OnPause();
        _moveBehavior.OnPause();
        _animator.SetFloat(AnimationSpeedParam, 0);
    }

    public void Resume()
    {
        _isPause = false;
        _currentState.Value.OnResume();
        _moveBehavior.OnResume();
        _animator.SetFloat(AnimationSpeedParam, GameManager.Instance.TimeController.EnemyTime);
    }

    public void Damage()
    {
        if (_isDefeated) return;

        _isDefeated = true;
        _performanceBehavior.Defeated(_moveBehavior.SpriteDir);
        gameObject.layer = LayerMask.NameToLayer(DefeatedTransitionLayerName);

        // 一定時間経過後、非表示にして画面外に移動させる
        DOVirtual.DelayedCall(Params.DefeatedStateTransitionDelay, () =>
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.one * 100;
        }).SetLink(gameObject);
    }
}
