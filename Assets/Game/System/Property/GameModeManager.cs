using UniRx;

public class GameModeManager
{
    private ReactiveProperty<GameMode> _currentgameMode = new ReactiveProperty<GameMode>(GameMode.NotSet);

    /// <summary>
    /// 現在のゲームモードを表現するリアクティブプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<GameMode> CurrentGameMode => _currentgameMode;

    /// <summary>
    /// ゲームモードの変更メソッド
    /// </summary>
    public void ChangeGameMode(GameMode nextMode)
    {
        _currentgameMode.Value = nextMode;
    }
}

/// <summary>
/// ゲームモードを表現する列挙型
/// </summary>
public enum GameMode
{
    NotSet,
    Title,
}