using UnityEngine;

/// <summary>
/// �ʏ�̓G�̊e�p�����[�^��ݒ肷��ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "EnemyParams_")]
public class EnemyParamsSO : ScriptableObject
{
    /// <summary>
    /// ���S�����ۂɎ��̂�������܂ł̒ǉ�����
    /// ���S�̃A�j���[�V�����ɒǉ��ő҂�
    /// </summary>
    static readonly float DefeatedStateTransitionDelayAdd = 3.0f;

    protected enum State
    {
        Idle,
        Search,
    }

    [Tooltip("Discover��Ԃ�AnimationClip�����蓖�Ă�")]
    [SerializeField] private AnimationClip _discoverAnimClip;
    [Tooltip("Dead��Ԃ�AnimationClip�����蓖�Ă�")]
    [SerializeField] private AnimationClip _deadAnimClip;

    [Header("�ړ����x�̐ݒ�")]
    [Tooltip("�����Ĉړ�����ۂ̑��x")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [Tooltip("�����Ĉړ�����ۂ̑��x")]
    [SerializeField] private float _runSpeed = 4.0f;

    [Header("Search��Ԃ̐܂�Ԃ��n�_�̐ݒ�")]
    [Tooltip("�܂�Ԃ��܂ł̋���")]
    [SerializeField] private float _turningPoint = 3.0f;
    [Tooltip("�܂�Ԃ��n�_�ɕt���O�Ƀ����_���ɐ܂�Ԃ�")]
    [SerializeField] private bool _useRandomTurningPoint;

    [Header("���E�̐ݒ�")]
    [Tooltip("���̎��E�̔��a")]
    [SerializeField] private float _sightRadius = 9.0f;
    [Tooltip("���̎��E�̊p�x")]
    [SerializeField] private float _sightAngle = 270.0f;
    [Tooltip("�Ԃɏ�Q�����������ꍇ�ɖ�������")]
    [SerializeField] private bool _isIgnoreObstacle;

    [Header("�U���͈͂̐ݒ�")]
    [Tooltip("�U���\�Ȕ͈�")]
    [SerializeField] private float _attackRange = 3.0f;
    [Tooltip("�U���̊Ԋu(�b)")]
    [SerializeField] private float _attackRate = 2.0f;

    [Header("Entry���̏��")]
    [SerializeField] protected State _entryState;
    [Header("�v���C���[���������͏��Idle��Ԃɂ���")]
    [SerializeField] protected bool _isIdleUndiscovered;

    public float DiscoverStateTransitionDelay => _discoverAnimClip != null ? _discoverAnimClip.length : 0;
    public float DefeatedStateTransitionDelay
    {
        get => _deadAnimClip != null ? _deadAnimClip.length + DefeatedStateTransitionDelayAdd : 0;
    }
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float TurningPoint => _turningPoint;
    public bool UseRandomTurningPoint => _useRandomTurningPoint;
    public float SightRadius => _sightRadius;
    public float SightAngle => _sightAngle;
    public bool IsIgnoreObstacle => _isIgnoreObstacle;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;
    public bool IsIdleUndiscovered => _isIdleUndiscovered;
    public virtual StateType EntryState
    {
        get
        {
            // �t���O�������Ă���ꍇ��Search��Ԃɂ����A���Idle��ԂƂȂ�
            if (_isIdleUndiscovered) return StateType.Idle;

            if (_entryState == State.Idle)
            {
                return StateType.Idle;
            }
            else
            {
                return StateType.Search;
            }
        }
    }

    // �������牺�̓v�����i�[�ɘM�点�Ȃ��l
    // �������A�v�]���������ۂɂ̓C���X�y�N�^�[�Ŋ��蓖�Ă���悤�ɕύX�\
    public float MinTransitionTimeElapsed => 1.0f;
    public float MaxTransitionTimeElapsed => 2.0f;
    public float MoveCancelTimerThreshold => 0.25f;

    public int GetAnimationHash(AnimationName name) => Animator.StringToHash(name.ToString());
}