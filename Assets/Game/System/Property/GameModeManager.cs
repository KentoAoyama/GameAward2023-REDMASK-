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
/// ゲームモードを表現する列挙型 <br/>
/// なんでこの型を用意したか忘れた。<br/>
/// 思い出したら必ずメモを残す。<br/>
/// しばらく経って思い出せなかったら削除する。（2023/02/28 記載）
/// </summary>
[System.Serializable]
public enum GameMode
{
    /// <summary> 未設定, エラー値を表現する値 </summary>
    NotSet,
    /// <summary> タイトル </summary>
    Title,
    /// <summary> ステージ選択 </summary>
    StageSelect,
    /// <summary> 準備画面（持っていく弾の選択や,ターゲット（ステージ）を選択する画面） </summary>
    PlayerSetup,
    /// <summary> ステージの開始演出 </summary>
    StartPerformance,
    /// <summary> ゲーム中を表現する値 </summary>
    InGame,
    /// <summary> リザルト </summary>
    Result,
    /// <summary> オプション（ボリューム調整, マップ確認やゲーム終了できる画面） </summary>
    Option,
}