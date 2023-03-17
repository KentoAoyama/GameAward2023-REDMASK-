using UniRx;

/// <summary>
/// 各ステートはこのクラスを用いて行動の処理を呼び出すメッセージの送信を行う
/// メッセージの受信はEnemyActorクラスが行う
/// </summary>
public class BehaviorMessenger
{
    private int _instanceID;

    public BehaviorMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// 各ステートはこのメソッドでメッセージを送信することで各行動の処理を呼び出す
    /// メッセージの受信はEnemyActorクラスが行う
    /// </summary>
    public void SendMessage(BehaviorType type)
    {
        MessageBroker.Default.Publish(new BehaviorMessage(type, _instanceID));
    }
}
