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
    private StageSelectManager _stageSelectManager = new StageSelectManager();
    private BulletsCountManager _bulletsCountManager = new BulletsCountManager();
    private SaveLoadManager _saveLoadManager = new SaveLoadManager();
    private CompletedStageManager _completedStageManager = new CompletedStageManager();
    private StageManager _stageManager = new StageManager();
    private AudioManager _audioManager = new AudioManager();
    private ShaderPropertyController _shaderPropertyController = new ShaderPropertyController();
    private GalleryManager _galleryManager = new GalleryManager();
    private CursorController _cursorController = new CursorController();

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
    /// <summary>
    /// ステージ選択状態をシーンを跨いで保存する用のクラス
    /// </summary>
    public StageSelectManager StageSelectManager => _stageSelectManager;
    /// <summary>
    /// 弾の所持数をシーンを跨いで保存する用のクラス
    /// </summary>
    public BulletsCountManager BulletsCountManager => _bulletsCountManager;
    /// <summary>
    /// セーブ
    /// </summary>
    public SaveLoadManager SaveLoadManager => _saveLoadManager;
    /// <summary>
    /// 完了済みステージを保存する用のクラス
    /// </summary>
    public CompletedStageManager CompletedStageManager => _completedStageManager;
    public StageManager StageManager => _stageManager;
    public AudioManager AudioManager => _audioManager;
    /// <summary>
    /// ShaderのPropertyを管理するクラス
    /// </summary>
    public ShaderPropertyController ShaderPropertyController => _shaderPropertyController;
    public GalleryManager GalleryManager => _galleryManager;
}