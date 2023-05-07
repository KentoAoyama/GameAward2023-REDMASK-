using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �ߐځA�������A�h���[���p
/// �e�U�镑���̃N���X�̃��\�b�h��g�ݍ��킹�čs���𐧌䂷��N���X
/// </summary>
[RequireComponent(typeof(SightSensor))]
[RequireComponent(typeof(MoveBehavior))]
[RequireComponent(typeof(AttackBehavior))]
[RequireComponent(typeof(PerformanceBehavior))]
public class EnemyController : MonoBehaviour, IPausable, IDamageable
{
    private static string AnimationSpeedParam = "Speed";
    private static string DefeatedTransitionLayerName = "DeathEnemy";

    [Header("�V�[����ɔz�u����Ă���v���C���[�̃^�O")]
    [SerializeField, TagName] private string _playerTagName;
    [Header("�G�̊e��p�����[�^�[��ݒ肵��SO")]
    [Tooltip("�e�U�镑���̃N���X�͂���SO���̒l���Q�Ƃ��ċ@�\����")]
    [SerializeField] protected EnemyParamsSO _enemyParamsSO;
    [Header("�������ɔz�u����")]
    [Tooltip("�����̃I�u�W�F�N�g�𔽓]������ꍇ�͎q��EditorView�I�u�W�F�N�g�����]������ƌ����ڂ�����")]
    [SerializeField] private bool _placedFacingLeft;
    [Header("�v���C���[���������͏��Idle��Ԃɂ���")]
    [SerializeField] private bool _idleWhenUndiscover;

    [Header("�f�o�b�O�p:���݂̏�Ԃ�\������Text")]
    [SerializeField] private Text _text;

    protected ReactiveProperty<StateTypeBase> _currentState = new();
    protected StateRegister _stateRegister = new();
    protected MoveBehavior _moveBehavior;
    private Transform _player;
    private SightSensor _sightSensor;
    private AttackBehavior _attackBehavior;
    private PerformanceBehavior _performanceBehavior;
    private Animator _animator;
    /// <summary>
    /// Pause()���Ă΂���true��Resume()���Ă΂���false�ɂȂ�
    /// </summary>
    private bool _isPause;
    /// <summary>
    /// ���j���ꂽ�ۂ�true�ɂȂ�Defeated��ԂɑJ�ڂ���
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
    /// ���̏�őҋ@����BIdle��Ԃ̎��A���t���[���Ă΂��
    /// </summary>
    public void UpdateIdle() => _moveBehavior.Idle();

    /// <summary>
    /// ���݂̈ړ����L�����Z�����ăv���C���[�Ɍ����Ĉړ�����
    /// Move��Ԃł̈ړ�������ۂɃX�e�[�g��Enter()�ŌĂ΂��
    /// </summary>
    public void MoveToPlayer()
    {
        _moveBehavior.CancelMoveToTarget();
        _moveBehavior.StartMoveToTarget(_player, Params.RunSpeed);
    }

    /// <summary>
    /// ���݂̈ړ����L�����Z�����Ď��͂̃����_���Ȍ��Ɉړ�����
    /// Search��Ԃł̈ړ�������ۂɃX�e�[�g��Enter()�ŌĂ΂��
    /// </summary>
    public void MoveSeachForPlayer()
    {
        _moveBehavior.CancelMoveToTarget();
        _moveBehavior.StartMoveSearchForPlayer(Params.RunSpeed, Params.TurningPoint, Params.UseRandomTurningPoint);
    }

    /// <summary>
    /// �J�ڂ���ۂɌ��݂̈ړ����L�����Z������ꍇ�ɃX�e�[�g����Ă΂��
    /// </summary>
    public void CancelMoving() => _moveBehavior.CancelMoveToTarget();

    /// <summary>
    /// ���E�ɑ΂��ăv���C���[���ǂ̈ʒu�ɂ���̂��𔻒肷��
    /// �e�X�e�[�g�̎��s���Ă΂ꑱ����
    /// </summary>
    public SightResult LookForPlayerInSight() => _sightSensor.LookForPlayerInSight(Params.SightRadius, 
        Params.SightAngle, Params.AttackRange, Params.IsIgnoreObstacle);

    /// <summary>
    /// Attack��Ԃ̎��A���Ԋu�ŌĂ΂��
    /// </summary>
    public virtual void Attack() => _attackBehavior.Attack();

    /// <summary>
    /// �e�X�e�[�g����Đ�����A�j���[�V�������Ăяo��
    /// </summary>
    public void PlayAnimation(AnimationName name) => _animator.Play(Params.GetAnimationHash(name));

    /// <summary>
    /// �������̉��o���s��
    /// </summary>
    public void PlayDiscoverPerformance() => _performanceBehavior.Discover();

    /// <summary>
    /// �e�X�e�[�g�͂��̃��\�b�h���ĂԂ��ƂőJ�ڐ�̃X�e�[�g���擾����
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

        // ��莞�Ԍo�ߌ�A��\���ɂ��ĉ�ʊO�Ɉړ�������
        DOVirtual.DelayedCall(Params.DefeatedStateTransitionDelay, () =>
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.one * 100;
        }).SetLink(gameObject);
    }
}
