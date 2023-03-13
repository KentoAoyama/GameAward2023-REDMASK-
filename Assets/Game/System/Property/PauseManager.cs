using System;
using UnityEngine;

/// <summary>
/// ポーズ, リジュームを管理するクラス
/// </summary>
public class PauseManager
{
    /// <summary> ポーズ命令が発行された回数をカウントする値 </summary>
    public int PauseCounter { get; private set; } = 0;

    private event Action OnPause = default;
    private event Action OnResume = default;

    /// <summary>
    /// ポーズ回数のカウンターをリセットする
    /// </summary>
    public void ClearCount()
    {
        PauseCounter = 0;
    }
    /// <summary>
    /// ポーズ, リジュームの登録処理
    /// </summary>
    /// <param name="pausable"> 登録するオブジェクト </param>
    public void Register(IPausable pausable)
    {
        // 現在ポーズ中であれば登録オブジェクトのポーズ処理を追加してから登録する。
        if (PauseCounter > 0)
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
    public void ExecutePause(Action onPause)
    {
        // ポーズ中でなければ登録されているポーズ処理を実行する。
        if (PauseCounter == 0)
        {
            Debug.Log("ポーズします。");
            OnPause?.Invoke();
            onPause?.Invoke();
        }
        else
        {
            //Debug.Log("既にポーズ中です。ポーズ命令が重複して発行されました。");
        }
        // ポーズ回数カウンターを加算する。
        PauseCounter++;

        // Debug.Log(現在のポーズ命令が発行された回数 : {_pauseCounter});
    }
    /// <summary>
    /// リジュームの実行処理
    /// </summary>
    public void ExecuteResume(Action onResume)
    {
        // ポーズ回数カウンターを減算する。（カウンターは0より小さくならない）
        if (PauseCounter >= 1)
        {
            PauseCounter--;
        }
        else
        {
            //Debug.LogWarning("リジューム命令が過剰に発行されました！\n" +
            //    "確認してください！");
        }

        // リジューム中でなければ登録されているリジューム処理を実行する。
        if (PauseCounter == 0)
        {
            Debug.Log("リジュームします。");
            OnResume?.Invoke();
            onResume?.Invoke();
        }
        else
        {
            //Debug.Log("既にリジューム中です。リジューム命令が重複して発行されました。");
        }
    }
}