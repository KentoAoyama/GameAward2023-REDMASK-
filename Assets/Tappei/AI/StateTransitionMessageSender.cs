using UniRx;

/// <summary>
/// ステート遷移のメッセージの送信を行う
/// メッセージの受信にはStateTransitionMessageReceiverクラスで行う
/// </summary>
public class StateTransitionMessageSender
{
    public StateTransitionMessageSender(int instanceID)
    {
        InstanceID = instanceID;
    }

    public int InstanceID { get; }

    /// <summary>
    /// 各機能のクラスはこのメソッドを呼ぶことでステートマシンに
    /// 遷移の条件を満たしたことを伝える
    /// </summary>
    public void SendMessage(StateTransitionTrigger trigger)
    {
        MessageBroker.Default.Publish(new StateTransitionMessage(trigger, InstanceID));
    }
}
