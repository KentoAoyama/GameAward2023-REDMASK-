// 日本語対応
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

/// <summary>
/// 完了演出終了時に実行する処理群
/// </summary>
public class StageComplete : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController = default;
    [SerializeField]
    private StageTimelineController timelineController = default;
    [Header("このステージの番号")]
    [SerializeField]
    private int _stageNumber = 0;

    private async void Awake()
    {
        await UniTask.WaitUntil(() => _playerController.IsSetUp);
        _playerController.gameObject.SetActive(false);
        timelineController.OnComplete += Save;
    }

    public void Save()
    {
        // 未使用の弾を返却
        _playerController.BulletCountManager.HomeBulletAddEndStage();
        // GameManagerが持つステージ用弾数,シリンダーの状態をリセット。
        GameManager.Instance.BulletsCountManager.Clear();
        // GameManager.BulletsCountManagerの情報を保存。
        GameManager.Instance.BulletsCountManager.Save();

        // 完了済みのステージ番号を更新する。
        GameManager.Instance.CompletedStageManager.SetCompletedStage(_stageNumber);
        // 完了済みのステージ番号を保存する。
        GameManager.Instance.CompletedStageManager.SaveStageCompleteNumber();
        // チェックポイントの情報を破棄
        GameManager.Instance.StageManager.Clear();
    }
}
