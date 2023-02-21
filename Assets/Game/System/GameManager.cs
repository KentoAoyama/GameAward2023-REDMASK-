// 日本語対応
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// ゲームの状態を表現するクラス
    /// </summary>
    public GameModeManager GameModeManager => _gameModeManager;
    /// <summary>
    /// ポーズ状態を表現するクラス
    /// </summary>
    public PauseManager PauseManager => _pauseManager;
}