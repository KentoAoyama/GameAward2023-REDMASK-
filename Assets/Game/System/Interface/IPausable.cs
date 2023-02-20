/// <summary>
/// ポーズ, リジューム可能である事を表現するインターフェース
/// </summary>
public interface IPausable
{
    public void Pause();
    public void Resume();
}