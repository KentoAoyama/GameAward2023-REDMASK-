using System;
using UnityEngine;

/// <summary>
/// ポーズ, リジュームを管理するクラス
/// </summary>
[Serializable]
public class PauseManager
{
    /// <summary>
    /// 現在のポーズ, リジューム状態を表現する値
    /// </summary>
    private PauseState _currentState = PauseState.NotSet;

    private event Action OnPause = default;
    private event Action OnResume = default;

    /// <summary>
    /// 現在のポーズ, リジューム状態を表現する値
    /// </summary>
    public PauseState CurrentState => _currentState;

    /// <summary>
    /// ポーズ, リジュームの登録処理
    /// </summary>
    /// <param name="pausable"> 登録するオブジェクト </param>
    public void Register(IPausable pausable)
    {
        if (_currentState == PauseState.Pause)
        {
            pausable.Pause();
        }
        OnPause += pausable.Pause;
        OnResume += pausable.Resume;
    }
    /// <summary>
    /// ポーズ, リジュームの解除処理
    /// </summary>
    /// <param name="pausable"> 登録済みのオブジェクト </param>
    public void Lift(IPausable pausable)
    {
        OnPause -= pausable.Pause;
        OnResume -= pausable.Resume;
    }
    /// <summary>
    /// ポーズの実行処理
    /// </summary>
    public void ExecutePause()
    {
        // ポーズ中でなければ登録されているポーズ処理を実行する。
        if (_currentState != PauseState.Pause)
        {
            _currentState = PauseState.Pause;
            OnPause?.Invoke();
        }
        else
        {
            Debug.LogWarning("既にポーズ中です。ポーズ命令が重複して発行されました。");
        }
    }
    /// <summary>
    /// リジュームの実行処理
    /// </summary>
    public void ExecuteResume()
    {
        // リジューム中でなければ登録されているリジューム処理を実行する。
        if (_currentState != PauseState.Resume)
        {
            _currentState = PauseState.Resume;
            OnResume?.Invoke();
        }
        else
        {
            Debug.LogWarning("既にリジューム中です。リジューム命令が重複して発行されました。");
        }
    }

    /// <summary>
    /// ポーズの状態を表す列挙型
    /// </summary>
    public enum PauseState
    {
        NotSet,
        Pause,
        Resume,
    }
}