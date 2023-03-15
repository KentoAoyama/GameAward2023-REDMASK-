using UniRx;

/// <summary>
/// 各ステートはこのクラスを用いて行動の処理を呼び出すメッセージの送信を行う
/// メッセージの受信は各機能が行う
/// </summary>
public class BehaviorMessenger
{
    private int _instanceID;

    public BehaviorMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// 各ステートはこのメソッドを呼ぶことで各機能を実行するメッセージを送信する
    /// メッセージの受信は各機能が行う
    /// </summary>
    public void SendMessage(BehaviorType type)
    {
        MessageBroker.Default.Publish(new BehaviorMessage(type, _instanceID));
    }
}
