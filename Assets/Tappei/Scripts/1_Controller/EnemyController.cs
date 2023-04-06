using UniRx;
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
    [Header("�V�[����ɔz�u����Ă���v���C���[�̃^�O")]
    [SerializeField, TagName] private string _playerTagName;
    [Header("�G�̊e��p�����[�^�[��ݒ肵��SO")]
    [Tooltip("�e�U�镑���̃N���X�͂���SO���̒l���Q�Ƃ��ċ@�\����")]
    [SerializeField] protected EnemyParamsSO _enemyParamsSO; 
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

    public EnemyParamsSO Params => _enemyParamsSO;

    /// <summary>
    /// ���j���ꂽ�ۂ�true�ɂȂ�t���O
    /// ���̃t���O����������Defeated��ԂɑJ�ڂ���
    /// </summary>
    public bool IsDefeated { get; private set; }

    protected virtual void Awake()
    {
        _sightSensor = gameObject.GetComponent<SightSensor>();
        _moveBehavior = gameObject.GetComponent<MoveBehavior>();
        _attackBehavior = gameObject.GetComponent<AttackBehavior>();
        _performanceBehavior = gameObject.GetComponent<PerformanceBehavior>();
        _animator = gameObject.GetComponentInChildren<Animator>();
        InitStateRegister();
        InitCurrentState();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
    }

    private void Update()
    {
        _currentState.Value = _currentState.Value.Execute();

        // �f�o�b�O�p
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

    /// <summary>
    /// �U������
    /// Attack��Ԃ̎��A���Ԋu�ŌĂ΂��
    /// </summary>
    public void Attack() => _attackBehavior.Attack();

    public void Idle() => _moveBehavior.Idle();

    /// <summary>
    /// �v���C���[�Ɍ����Ĉړ�����
    /// Move��Ԃł̈ړ�������ۂɃX�e�[�g��Enter()�ŌĂ΂��
    /// </summary>
    public void MoveToPlayer()
    {
        _moveBehavior.CancelMoving();
        _moveBehavior.StartMoveToTarget(_player, Params.RunSpeed);
    }

    /// <summary>
    /// ���͂̃����_���Ȍ��Ɉړ�����
    /// Search��Ԃł̈ړ�������ۂɃX�e�[�g��Enter()�ŌĂ΂��
    /// </summary>
    public void SearchMoving()
    {
        _moveBehavior.CancelMoving();
        Transform target = _moveBehavior.GetSearchDestination(
            Params.TurningPoint, Params.UseRandomTurningPoint);
        _moveBehavior.StartMoveToTarget(target, Params.WalkSpeed);
    }

    /// <summary>
    /// �J�ڂ���ۂɌ��݂̈ړ����L�����Z�����邽�߂ɃX�e�[�g����Ă΂��
    /// </summary>
    public void CancelMoving() => _moveBehavior.CancelMoving();

    /// <summary>
    /// ���E�ɑ΂��ăv���C���[���ǂ̈ʒu�ɂ���̂��𔻒肷��
    /// �e�X�e�[�g�̎��s���Ă΂ꑱ����
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

    /// <summary>�e�X�e�[�g���Đ�����A�j���[�V�������Ăяo��</summary>
    public void PlayAnimation(AnimationName name) => _animator.Play(Params.GetAnimationHash(name));

    public void DefeatedPerformance() => _performanceBehavior.Defeated();
    public void DiscoverPerformance() => _performanceBehavior.Discover();

    /// <summary>
    /// �e�X�e�[�g�͂��̃��\�b�h���ĂԂ��ƂőJ�ڐ�̃X�e�[�g���擾����
    /// </summary>
    public StateTypeBase GetState(StateType type) => _stateRegister.GetState(type);

    public void Pause()
    {
        // �e�U�镑���̃|�[�Y�������܂Ƃ߂ČĂ�
    }

    public void Resume()
    {
        // �e�U�镑���̃|�[�Y�����������܂Ƃ߂ČĂ�
    }

    /// <summary>���j���ꂽ�ۂ͔�\���ɂ��ĉ�ʊO�Ɉړ�������</summary>
    public void Damage()
    {
        IsDefeated = true;
        _performanceBehavior.Defeated();

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
