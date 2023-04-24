using UniRx;
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
    [Header("デバッグ用:現在の状態を表示するText")]
    [SerializeField] private Text _text;

    protected ReactiveProperty<StateTypeBase> _currentState = new();
    protected StateRegister _stateRegister = new();
    protected MoveBehavior _moveBehavior;
    private Transform _player;
    private SightSensor _sightSensor;
    private AttackBehavior _attackBehavior;
    private PerformanceBehavior _performanceBehavior;
    private Animator _animator;

    public EnemyParamsSO Params => _enemyParamsSO;

    /// <summary>Pause()が呼ばれるとtrueにResume()が呼ばれるとfalseになる</summary>
    private bool _isPause;

    /// <summary>
    /// 撃破された際にtrueになるフラグ
    /// このフラグが立ったらDefeated状態に遷移する
    /// </summary>
    public bool IsDefeated { get; private set; }

    protected virtual void Awake()
    {
        _sightSensor = GetComponent<SightSensor>();
        _moveBehavior = GetComponent<MoveBehavior>();
        _attackBehavior = GetComponent<AttackBehavior>();
        _performanceBehavior = GetComponent<PerformanceBehavior>();
        _animator = GetComponentInChildren<Animator>();
        InitStateRegister();
        InitCurrentState();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
        GameManager.Instance.PauseManager.Register(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Lift(this);
    }

    private void Update()
    {
        if (_isPause) return;

        _currentState.Value = _currentState.Value.Execute();
        _animator.SetFloat(AnimationSpeedParam, GameManager.Instance.TimeController.EnemyTime);

        // デバッグ用
        if (_text != null)
        {
            _text.text = _currentState.Value.ToString();
        }
    }

    protected virtual void InitStateRegister()
    {
        _stateRegister.Register(StateType.Idle, this);
        _stateRegister.Register(StateType.Search, this);
        _stateRegister.Register(StateType.Discover, this);
        _stateRegister.Register(StateType.Move, this);
        _stateRegister.Register(StateType.Attack, this);
        _stateRegister.Register(StateType.Defeated, this);
    }

    private void InitCurrentState()
    {
        StateType state = Params.EntryState;
        _currentState.Value = _stateRegister.GetState(state);
    }

    /// <summary>その場で攻撃する。Attack状態の時、一定間隔で呼ばれる</summary>
    public virtual void Attack() => _attackBehavior.Attack();

    /// <summary>その場で待機する。Idle状態の時、毎フレーム呼ばれる</summary>
    public void Idle() => _moveBehavior.Idle();

    /// <summary>
    /// プレイヤーに向けて移動する
    /// Move状態での移動をする際にステートのEnter()で呼ばれる
    /// </summary>
    public void MoveToPlayer()
    {
        _moveBehavior.CancelMoving();
        _moveBehavior.StartMoveToTarget(_player, Params.RunSpeed);
    }

    /// <summary>
    /// 周囲のランダムな個所に移動する
    /// Search状態での移動をする際にステートのEnter()で呼ばれる
    /// </summary>
    public void SearchMoving()
    {
        _moveBehavior.CancelMoving();
        Transform target = _moveBehavior.GetSearchDestination(
            Params.TurningPoint, Params.UseRandomTurningPoint);
        _moveBehavior.StartMoveToTarget(target, Params.WalkSpeed);
    }

    /// <summary>
    /// 遷移する際に現在の移動をキャンセルするためにステートから呼ばれる
    /// </summary>
    public void CancelMoving() => _moveBehavior.CancelMoving();

    /// <summary>
    /// 視界に対してプレイヤーがどの位置にいるのかを判定する
    /// 各ステートの実行中呼ばれ続ける
    /// </summary>
    public SightResult IsFindPlayer()
    {
        float distance = _sightSensor.TryGetDistanceToPlayer(
            Params.SightRadius, Params.SightAngle, Params.IsIgnoreObstacle);

        if (distance == SightSensor.PlayerOutSight)
        {
            return SightResult.OutSight;
        }
        else if (distance < Params.AttackRange)
        {
            return SightResult.InAttackRange;
        }
        else
        {
            return SightResult.InSight;
        }
    }

    /// <summary>各ステートが再生するアニメーションを呼び出す</summary>
    public void PlayAnimation(AnimationName name) => _animator.Play(Params.GetAnimationHash(name));

    //public void DefeatedPerformance() => _performanceBehavior.Defeated();
    public void DiscoverPerformance() => _performanceBehavior.Discover();

    /// <summary>
    /// 各ステートはこのメソッドを呼ぶことで遷移先のステートを取得する
    /// </summary>
    public StateTypeBase GetState(StateType type) => _stateRegister.GetState(type);

    public void Pause()
    {
        _isPause = true;
        _currentState.Value.Pause();
        _moveBehavior.Pause();
        _animator.SetFloat(AnimationSpeedParam, 0);
    }

    public void Resume()
    {
        _isPause = false;
        _currentState.Value.Resume();
        _moveBehavior.Resume();
        _animator.SetFloat(AnimationSpeedParam, GameManager.Instance.TimeController.EnemyTime);
    }

    /// <summary>撃破された際は非表示にして画面外に移動させる</summary>
    public void Damage()
    {
        if (IsDefeated) return;

        IsDefeated = true;
        _performanceBehavior.Defeated(_moveBehavior.SpriteDirection);
        gameObject.layer = LayerMask.NameToLayer(DefeatedTransitionLayerName);

        DOVirtual.DelayedCall(Params.DefeatedStateTransitionDelay, () =>
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.one * 100;
        }).SetLink(gameObject);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            DrawTurningPoint();
            DrawSight();
            DrawAttackRange();
        }
    }

    private void DrawTurningPoint()
    {
        float turningPoint = Params.TurningPoint;
        Vector3 footPos = _moveBehavior.FootPos;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(footPos + Vector3.right * turningPoint, 0.25f);
        Gizmos.DrawWireSphere(footPos + Vector3.left * turningPoint, 0.25f);
    }

    private void DrawSight()
    {
        Transform eye = _sightSensor.EyeTransform;
        Vector3 dir = Quaternion.Euler(0, 0, -Params.SightAngle / 2) * eye.right;

        UnityEditor.Handles.color = new Color32(0, 0, 255, 64);
        UnityEditor.Handles.DrawSolidArc(eye.position, Vector3.forward, dir, 
            Params.SightAngle, Params.SightRadius);
    }

    private void DrawAttackRange()
    {
        Transform eye = _sightSensor.EyeTransform;
        Vector3 dir = Quaternion.Euler(0, 0, -Params.SightAngle / 2) * eye.right;

        UnityEditor.Handles.color = new Color32(255, 0, 0, 64);
        UnityEditor.Handles.DrawSolidArc(eye.position, Vector3.forward, dir,
            Params.SightAngle, Params.AttackRange);
    }
#endif
}
