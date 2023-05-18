using UniRx;

/// <summary>
/// 台詞を表示させるのに使用するクラス
/// 送る側
/// </summary>
public static class LineMessageSender
{
    public static void SendMessage(string line)
    {
        MessageBroker.Default.Publish(new LineMessage(line));
    }
}
