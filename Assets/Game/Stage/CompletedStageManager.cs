// 日本語対応
using UnityEngine;

public class CompletedStageManager
{
    private int _maxCompletedStageNumber = 0;
    /// <summary>
    /// 完了済みのステージ番号のPlayerPrefsのキー
    /// </summary>
    private string CompletedStageNumber = "CompletedStageNumber";

    /// <summary>
    /// ステージ完了演出 開始時か完了時に 実行するメソッド <br/>
    /// 完了済みステージの最大番号を保存する。
    /// </summary>
    public void SaveStageCompleteNumber()
    {
        PlayerPrefs.SetInt(CompletedStageNumber, _maxCompletedStageNumber);
    }
    /// <summary>
    /// 完了済みステージの最大番号を読み込む
    /// </summary>
    public int LoadStageCompleteNumber()
    {
        return _maxCompletedStageNumber = PlayerPrefs.GetInt(CompletedStageNumber, 0);
    }
    public void SetCompletedStage(int stageNumber)
    {
        if (_maxCompletedStageNumber < stageNumber)
        {
            _maxCompletedStageNumber = stageNumber;
        }
    }
    public int GetMaxCompletedStageNumber()
    {
        return _maxCompletedStageNumber;
    }
    public void Clear()
    {
        _maxCompletedStageNumber = 0;
    }
}
