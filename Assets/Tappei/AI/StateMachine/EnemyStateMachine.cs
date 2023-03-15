using UniRx;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V��
/// </summary>
[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    // TODO:SO�ɂ��ĊǗ�����N���X�Ɏ������������ǂ�(FlyWeight�p�^�[��)
    private StateTransitionFlow _stateTransitionFlow;
    private ReactiveProperty<StateTypeBase> _currentState = new();
    private StateRegister _stateRegister;

    public IReadOnlyReactiveProperty<StateTypeBase> CurrentState => _currentState;

    private void Awake()
    {
        _stateTransitionFlow = GetComponent<StateTransitionFlow>();
        InitMessageReceive();
        InitState();
        SetDefaultState(StateType.Idle);
    }

    private void Update()
    {
        UpdateCurrentState();
    }

    /// <summary>
    /// �e�@�\���J�ڂ̏����𖞂������ۂ�StateTransitionMessenger�N���X����
    /// ���M����郁�b�Z�[�W����M������J�ڂ��s��
    /// </summary>
    private void InitMessageReceive()
    {
        MessageBroker.Default.Receive<StateTransitionMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Subscribe(message => StateTransition(message.Trigger)).AddTo(this);
    }

    private void InitState()
    {
        StateMachineHelper helper = new();
        // TODO:�X�e�[�g�}�V�����n���Ă��邪�A�X�e�[�g���ŕs�v�Ȃ�n���Ȃ��Ă悢
        _stateRegister = new(this, helper);
        _stateRegister.Register(StateType.Idle);
        _stateRegister.Register(StateType.Search);
    }

    private void SetDefaultState(StateType type) => _currentState.Value = _stateRegister.GetState(type);

    private void UpdateCurrentState() => _currentState.Value = _currentState.Value.Execute();

    /// <summary>
    /// �e�@�\���烁�b�Z�[�W����M������J�ڂ��s���̂�
    /// Update()�ł̃X�e�[�g�̍X�V�Ƃ͕ʂ̃^�C�~���O�ŌĂ΂��
    /// </summary>
    private void StateTransition(StateTransitionTrigger trigger)
    {
        StateType current = _currentState.Value.StateType;
        StateType next = _stateTransitionFlow.GetNextStateType(current, trigger);

        StateTypeBase nextState = _stateRegister.GetState(next);
        _currentState.Value.TryChangeState(nextState);
    }

    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}