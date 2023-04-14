// 日本語対応
using Player;
using UnityEngine;

/// <summary>
/// ステージの状態を管理 監督するオブジェクト。
/// </summary>
public class StageController : MonoBehaviour
{
    [Header("プレイヤーのタグ")]
    [TagName, SerializeField]
    private string _playerTag = default;
    [Header("各演出用オブジェクトを割り当ててください")]
    [Tooltip("ステージ開始演出用オブジェクト"), SerializeField]
    private StageStartEventBase _stageStartPerformanceObjct = default;
    [Tooltip("ステージクリア演出用オブジェクト"), SerializeField]
    private StageCompleteEventBase _stageCompletePerformanceObject = default;
    [Tooltip("プレイヤー死亡演出用オブジェクト"), SerializeField]
    private PlayerDeadEventBase _playerDeadPerformanceObjct = default;

    private void Awake()
    {
    }

    /// <summary> ステージの完了演出を再生する </summary>
    public void StageComplete()
    {
        if (_playerDeadPerformanceObjct.gameObject.activeSelf == false)
        {
            _stageCompletePerformanceObject.gameObject.SetActive(true);
        } // ステージクリア演出かプレイヤー死亡演出はどちらか一方しかアクティブにできない。
    }
    /// <summary> プレイヤー死亡時処理を再生する </summary>
    public void PlayerDead()
    {
        if (_stageCompletePerformanceObject.gameObject.activeSelf == false)
        {
            _playerDeadPerformanceObjct.gameObject.SetActive(true);
        } // ステージクリア演出かプレイヤー死亡演出はどちらか一方しかアクティブにできない。
    }
}
