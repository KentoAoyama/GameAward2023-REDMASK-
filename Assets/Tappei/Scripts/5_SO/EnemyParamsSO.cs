using UnityEngine;

/// <summary>
/// �ʏ�̓G�̊e�p�����[�^��ݒ肷��ScriptableObject
/// EnemyParamsManager�Ɏ������A�G���ɎQ�Ƃ���
/// </summary>
[CreateAssetMenu(fileName = "EnemyParams_")]
public class EnemyParamsSO : ScriptableObject
{
    enum State
    {
        Idle,
        Search,
    }

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
    [SerializeField] private State _entryState;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float TurningPoint => _turningPoint;
    public bool UseRandomTurningPoint => _useRandomTurningPoint;
    public float SightRadius => _sightRadius;
    public float SightAngle => _sightAngle;
    public bool IsIgnoreObstacle => _isIgnoreObstacle;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;
    public StateType EntryState
    {
        get
        {
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
}
