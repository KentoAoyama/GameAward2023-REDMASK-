// 日本語対応
using UnityEngine;

public class SaveCompletedStageNumber : MonoBehaviour
{
    [SerializeField]
    private int _stageNumber = 0;

    public void OnStageCompleted()
    {
        // 完了済みステージ番号を更新する。
        GameManager.Instance.CompletedStageManager.SetCompletedStage(_stageNumber);
        // 状態を保存する。
        GameManager.Instance.CompletedStageManager.SaveStageCompleteNumber();
    }
}
