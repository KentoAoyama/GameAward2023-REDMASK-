using UniRx;

/// <summary>
/// �䎌��\��������̂Ɏg�p����N���X
/// ���鑤
/// </summary>
public static class LineMessageSender
{
    public static void SendMessage(string line)
    {
        MessageBroker.Default.Publish(new LineMessage(line));
    }
}
