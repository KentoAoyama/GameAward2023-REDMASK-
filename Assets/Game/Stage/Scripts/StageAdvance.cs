// 日本語対応
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージを進める用コンポーネント
/// </summary>
public class StageAdvance : MonoBehaviour
{
    [Header("ステージ管理コンポーネント")]
    [SerializeField]
    private StageController2 _stageController2 = default;
    [Header("プレイヤーのタグ")]
    [TagName, SerializeField]
    private string _playerTag = default;
    [Header("次のシーンの名前")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;
    [Header("この面でこのステージは最後かどうか")]
    [SerializeField]
    private bool _isCompletedAtThisStage = default;
    [Header("このステージの番号（ステージクリア時用。\n上記のチェックボックスにチェックがついている時のみ使用する。）")]
    [SerializeField]
    private int _stageNumber = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと接触したとき実行する。
        if (collision.tag == _playerTag) 
        {
            // この面が最後であれば 必要な処理を実行する。
            if (_isCompletedAtThisStage)
            {
                CompletedStage();
            }
            // シーンを更新する。
            SceneManager.LoadScene(_nextSceneName);
        }
    }
    /// <summary>
    /// ステージクリア時用メソッド
    /// </summary>
    public void CompletedStage()
    {
        // 余った弾をホームに返す。
        var player = GameObject.FindGameObjectWithTag(_playerTag);
        var playerController = player.GetComponent<PlayerController>();
        playerController.StageComplete();
        // 完了済みのステージを更新する。
        GameManager.Instance.CompletedStageManager.SetCompletedStage(_stageNumber);
        // 状態を保存する。
        GameManager.Instance.CompletedStageManager.SaveStageCompleteNumber();
    }
}
