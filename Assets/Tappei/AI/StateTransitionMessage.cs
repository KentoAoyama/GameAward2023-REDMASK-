/// <summary>
/// この構造体のメッセージを送受信することでステートの遷移を行う
/// StateTransitionMessageSender/Receiverクラスで送信/受信をする
/// </summary>
public struct StateTransitionMessage
{
    public StateTransitionMessage(StateTransitionTrigger trigger, int id)
    {
        Trigger = trigger;
        ID = id;
    }

    public StateTransitionTrigger Trigger { get; }
    public int ID { get; }
}