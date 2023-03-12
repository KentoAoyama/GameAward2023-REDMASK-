using UniRx;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V��
/// </summary>
[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    [SerializeField] StateTransitionFlow _stateTransitionFlow;

    private ReactiveProperty<StateTypeBase> _currentState = new();
    private EnemyStateRegister _stateRegister;

    public IReadOnlyReactiveProperty<StateTypeBase> CurrentState;

    private int _instanceID;

    private void Awake()
    {
        _instanceID = gameObject.GetInstanceID();

        MessageBroker.Default.Receive<StateTransitionMessage>()
            .Where(message => message.ID == _instanceID)
            .Subscribe(message => Receive(message.Trigger)).AddTo(this);

        EnemyStateMachineHelper helper = new();
        // TODO:�X�e�[�g�}�V�����n���Ă��邪�A�X�e�[�g���ŕs�v�Ȃ�n���Ȃ��Ă悢
        _stateRegister = new(this, helper);
        _stateRegister.Register(StateType.Idle);
        _stateRegister.Register(StateType.Search);

        _currentState.Value = _stateRegister.GetState(StateType.Idle);
    }

    void Start()
    {
        // ��ԑJ�ڂɎg�����́A�����J�ڂ݂̂������āA�J�ڂ��邩��Ԃ��N���X��p�ӂ���
        // �v���C���[�𔭌�����
        // �v���C���[����������
        // �_���[�W���󂯂�
        // �̗͂����ȉ��ɂȂ���
        // ��莞�Ԃ�������

        // Idle
        // ��莞�Ԍo�߁cSearch�ɑJ��
        // �v���C���[�����cMove�ɑJ��
        
        // Search
        // ��莞�Ԍo�߁cIdle�ɑJ��
        // �v���C���[�����cMove�ɑJ��

        // Move
        // �v���C���[�Ƃ̋������U���͈͓��cAttack�ɑJ��
        // �v���C���[�����������cSearch�ɑJ��

        // Attack
        // �v���C���[�Ƃ̋������U���͈͊O�cMove�ɑJ��
        // �v���C���[�����������cSearch�ɑJ��

        // �e�X�e�[�g�ɑ΂���
        // ����n����������X�e�[�gs�ɑJ�ڂ���
        // �Ƃ������̂������ق���
        // �����ƑJ�ڐ�̓y�A
    }

    void Update()
    {
        _currentState.Value = _currentState.Value.Execute();
    }

    /// <summary>
    /// ���b�Z�[�W����M������J�ڏ������s��
    /// </summary>
    void Receive(StateTransitionTrigger trigger)
    {
        StateType type = _currentState.Value.StateType;

        StateType next = _stateTransitionFlow.GetNextState(type, trigger);
        StateTypeBase nextState = _stateRegister.GetState(next);
        _currentState.Value.TryChangeState(nextState);
    }

    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}