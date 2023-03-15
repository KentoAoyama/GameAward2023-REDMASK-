/// <summary>
/// この構造体のメッセージを送受信することで各機能が呼び出す
/// BehaviorMessengerクラスから送信され、各機能が受信する
/// </summary>
public struct BehaviorMessage
{
    public BehaviorMessage(BehaviorType type, int id)
    {
        Type = type;
        ID = id;
    }

    public BehaviorType Type { get; }
    public int ID { get; }
}
