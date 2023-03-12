using UniRx;
using UnityEngine;

/// <summary>
/// 敵のステートマシン
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
        // TODO:ステートマシンも渡しているが、ステート側で不要なら渡さなくてよい
        _stateRegister = new(this, helper);
        _stateRegister.Register(StateType.Idle);
        _stateRegister.Register(StateType.Search);

        _currentState.Value = _stateRegister.GetState(StateType.Idle);
    }

    void Start()
    {
        // 状態遷移に使うもの、条件遷移のみを書いて、遷移するかを返すクラスを用意する
        // プレイヤーを発見した
        // プレイヤーを見失った
        // ダメージを受けた
        // 体力が一定以下になった
        // 一定時間がたった

        // Idle
        // 一定時間経過…Searchに遷移
        // プレイヤー発見…Moveに遷移
        
        // Search
        // 一定時間経過…Idleに遷移
        // プレイヤー発見…Moveに遷移

        // Move
        // プレイヤーとの距離が攻撃範囲内…Attackに遷移
        // プレイヤーを見失った…Searchに遷移

        // Attack
        // プレイヤーとの距離が攻撃範囲外…Moveに遷移
        // プレイヤーを見失った…Searchに遷移

        // 各ステートに対して
        // 条件nが揃ったらステートsに遷移する
        // というものが複数ほしい
        // 条件と遷移先はペア
    }

    void Update()
    {
        _currentState.Value = _currentState.Value.Execute();
    }

    /// <summary>
    /// メッセージを受信したら遷移処理を行う
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