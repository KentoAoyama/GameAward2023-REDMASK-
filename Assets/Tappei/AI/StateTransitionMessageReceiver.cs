using System;
using UniRx;
using UnityEngine;

/// <summary>
/// StateTransitionMessageSenderから送信されたメッセージを受信する
/// </summary>
public class StateTransitionMessageReceiver : MonoBehaviour
{
    [Serializable]
    private struct TransitionFlow
    {
        [SerializeField] private EnemyStateType _currentState;
        [SerializeField] private EnemyStateType _nextstate;
        [SerializeField] private StateTransitionTrigger _message;

        public EnemyStateType CurrentState => _currentState;
        public EnemyStateType Nextstate => _nextstate;
        public StateTransitionTrigger Message => _message;
    }

    [Header("遷移元/遷移先/遷移条件")]
    [SerializeField] TransitionFlow[] _transitionFlow;

    private int _instanceID;

    private void Awake()
    {
        _instanceID = gameObject.GetInstanceID();

        // 同じインスタンスから送信されたメッセージのみを受信させる
        MessageBroker.Default.Receive<StateTransitionMessage>()
            .Where(message => message.ID == _instanceID)
            .Subscribe(message => Debuga(message)).AddTo(this);
    }

    public void Execute(EnemyStateType currentState)
    {
        // 飛んできたメッセージは遷移のためのトリガー
        // このメソッドは毎フレーム呼ばれる
        

        // 現在のステートが渡される
        // 同一フレームに複数のメッセージが送られてくる場合があるので対処する必要がある？
        // メッセージが飛んできていない場合はそのステートを返す
        // メッセージが飛んで来たら渡されたステートと遷移条件で辞書に検索をかける
    }

    private void Debuga(StateTransitionMessage message)
    {
        Debug.Log(message.Trigger + "を受信: "+message.ID);
    }
}
