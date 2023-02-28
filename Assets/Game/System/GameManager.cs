// 日本語対応
using UnityEngine;

/// <summary>
/// ゲームの管理者
/// </summary>
public class GameManager
{
    #region Singleton
    private static GameManager _instance = new GameManager();
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }
    private GameManager() { }
    #endregion

    private GameModeManager _gameModeManager = new GameModeManager();
    private PauseManager _pauseManager = new PauseManager();
    private TimeController _timeController = new TimeController();

    /// <summary>
    /// ゲームの状態を表現するクラス
    /// </summary>
    public GameModeManager GameModeManager => _gameModeManager;
    /// <summary>
    /// ポーズ状態を表現するクラス
    /// </summary>
    public PauseManager PauseManager => _pauseManager;
    /// <summary>
    /// 現在のゲーム内時間の速度を管理･提供するクラス
    /// </summary>
    public TimeController TimeController => _timeController;
}