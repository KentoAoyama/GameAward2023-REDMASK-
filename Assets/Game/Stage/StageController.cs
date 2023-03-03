// 日本語対応
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("各演出用オブジェクトを割り当ててください")]
    [Tooltip("ステージ開始演出用オブジェクト"), SerializeField]
    private StageStartEventBase _stageStartPerformanceObjct = default;
    [Tooltip("ステージクリア演出用オブジェクト"), SerializeField]
    private StageCompleteEventBase _stageCompletePerformanceObject = default;
    [Tooltip("プレイヤー死亡演出用オブジェクト"), SerializeField]
    private PlayerDeadEventBase _playerDeadPerformanceObjct = default;

    private void Awake()
    {
        // 開始演出用オブジェクトをアクティブに、
        // 完了演出、死亡時演出用オブジェクトを非アクティブにする。
        _stageStartPerformanceObjct.gameObject.SetActive(true);
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
