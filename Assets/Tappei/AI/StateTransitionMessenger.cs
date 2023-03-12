using UniRx;

/// <summary>
/// ステート遷移のメッセージの送信を行うクラス
/// メッセージの受信はステートマシンが行う
/// </summary>
public class StateTransitionMessenger
{
    private int _instanceID;

    public StateTransitionMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// 各機能のクラスはこのメソッドを呼ぶことでステートマシンに
    /// 遷移の条件を満たしたというメッセージを送信する
    /// </summary>
    public void SendMessage(StateTransitionTrigger trigger)
    {
        MessageBroker.Default.Publish(new StateTransitionMessage(trigger, _instanceID));
    }
}
