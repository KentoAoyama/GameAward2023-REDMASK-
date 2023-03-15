using UniRx;
using UnityEngine;

/// <summary>
/// 敵のステートマシン
/// </summary>
[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    // TODO:SOにして管理するクラスに持たせた方が良い(FlyWeightパターン)
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
    /// 各機能が遷移の条件を満たした際にStateTransitionMessengerクラスから
    /// 送信されるメッセージを受信したら遷移を行う
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
        // TODO:ステートマシンも渡しているが、ステート側で不要なら渡さなくてよい
        _stateRegister = new(this, helper);
        _stateRegister.Register(StateType.Idle);
        _stateRegister.Register(StateType.Search);
    }

    private void SetDefaultState(StateType type) => _currentState.Value = _stateRegister.GetState(type);

    private void UpdateCurrentState() => _currentState.Value = _currentState.Value.Execute();

    /// <summary>
    /// 各機能からメッセージを受信したら遷移を行うので
    /// Update()でのステートの更新とは別のタイミングで呼ばれる
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