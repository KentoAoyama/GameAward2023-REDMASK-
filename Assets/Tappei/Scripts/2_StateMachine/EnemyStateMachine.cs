using UniRx;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵のステートマシン
/// </summary>
[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    [Header("Searchをデフォルトのステートにするか")]
    [SerializeField] bool _isSearchStateDefault;

    // TODO:SOにして管理するクラスに持たせた方が良い(FlyWeightパターン)
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
    /// 各機能が遷移の条件を満たした際にStateTransitionMessengerクラスから
    /// 送信されるメッセージを受信したら遷移を行う
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
    /// 各機能からメッセージを受信したら遷移を行うので
    /// Update()でのステートの更新とは別のタイミングで呼ばれる
    /// </summary>
    private void StateTransition(StateTransitionTrigger trigger)
    {
        StateType current = _currentState.Value.Type;
        StateType next = _stateTransitionFlow.GetNextStateType(current, trigger);

        // 値がUnknownの場合は遷移先が登録されていないが、意図したものである場合が多い
        // どのステートでもあらゆる遷移のメッセージを受信しているのでここで弾く必要がある
        if (next == StateType.Unknown) return;

        StateTypeBase nextState = _stateRegister.GetState(next);
        _currentState.Value.TryChangeState(nextState);
    }

    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}