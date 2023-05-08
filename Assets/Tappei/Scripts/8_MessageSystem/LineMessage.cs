/// <summary>
/// 表示される台詞の構造体
/// これをメッセージングでやり取りする
/// </summary>
public readonly struct LineMessage
{
    public LineMessage(string line)
    {
        Line = line;
    }

    public string Line { get; }
}
