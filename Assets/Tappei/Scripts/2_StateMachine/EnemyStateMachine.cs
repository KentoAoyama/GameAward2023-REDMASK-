using UniRx;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �G�̃X�e�[�g�}�V��
/// </summary>
[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    [Header("Search���f�t�H���g�̃X�e�[�g�ɂ��邩")]
    [SerializeField] bool _isSearchStateDefault;

    // TODO:SO�ɂ��ĊǗ�����N���X�Ɏ������������ǂ�(FlyWeight�p�^�[��)
    private StateTransitionFlow _stateTransitionFlow;
    private ReactiveProperty<StateTypeBase> _currentState = new();
    private StateRegister _stateRegister;

    public IReadOnlyReactiveProperty<StateTypeBase> CurrentState => _currentState;

    private void Awake()
    {
        _stateTransitionFlow = GetComponent<StateTransitionFlow>();
        InitMessageReceive();
        InitStateRegister();
        SetDefaultState();
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

    private void InitStateRegister()
    {
        //BehaviorMessenger messenger = new(gameObject.GetInstanceID());
        //StateMachineHelper helper = new();
        //_stateRegister = new(messenger, helper);
        //_stateRegister.Register(StateType.Idle);
        //_stateRegister.Register(StateType.Search);
        //_stateRegister.Register(StateType.Move);
        //_stateRegister.Register(StateType.Attack);
        //_stateRegister.Register(StateType.Defeated);
    }

    private void SetDefaultState()
    {
        StateType state = _isSearchStateDefault ? StateType.Search : StateType.Idle;
        _currentState.Value = _stateRegister.GetState(state);
    }

    private void UpdateCurrentState()
    {
        _currentState.Value = _currentState.Value.Execute();
    }

    /// <summary>
    /// �e�@�\���烁�b�Z�[�W����M������J�ڂ��s���̂�
    /// Update()�ł̃X�e�[�g�̍X�V�Ƃ͕ʂ̃^�C�~���O�ŌĂ΂��
    /// </summary>
    private void StateTransition(StateTransitionTrigger trigger)
    {
        StateType current = _currentState.Value.Type;
        StateType next = _stateTransitionFlow.GetNextStateType(current, trigger);

        // �l��Unknown�̏ꍇ�͑J�ڐ悪�o�^����Ă��Ȃ����A�Ӑ}�������̂ł���ꍇ������
        // �ǂ̃X�e�[�g�ł�������J�ڂ̃��b�Z�[�W����M���Ă���̂ł����Œe���K�v������
        if (next == StateType.Unknown) return;

        StateTypeBase nextState = _stateRegister.GetState(next);
        _currentState.Value.TryChangeState(nextState);
    }

    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}