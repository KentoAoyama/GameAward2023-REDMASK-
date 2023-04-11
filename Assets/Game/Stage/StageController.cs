// 日本語対応
using Player;
using UnityEngine;

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
        // ステージの開始処理
        // 完了演出、死亡時演出用オブジェクトを非アクティブにする。
        _stageCompletePerformanceObject.gameObject.SetActive(false);
        _playerDeadPerformanceObjct.gameObject.SetActive(false);
        if (GameManager.Instance.StageManager.StageStartMode == StageStartMode.FromTheBeginning)
        {
            // チェックポイントの情報をリセット
            GameManager.Instance.StageManager.IsTouchCheckPoint = false;
            // 開始演出用オブジェクトをアクティブにする。
            _stageStartPerformanceObjct.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.StageManager.StageStartMode == StageStartMode.JustBefore)
        {
            // チェックポイントを一つでも触れていなければ、チェックポイント通過時の状態を復元する。
            // 一つも触れていなければ、何もしない。
            // チェックポイントからやり直す
            if (GameManager.Instance.StageManager.IsTouchCheckPoint)
            {
                // プレイヤーオブジェクト取得
                var player = GameObject.FindGameObjectWithTag(_playerTag);
                // ポジション設定
                player.transform.position =
                    GameManager.Instance.StageManager.LastCheckPointPosition;


                // プレイヤーコントローラー取得
                var playerController = player.GetComponent<PlayerController>();
                // シリンダーの状態を復元
                // playerController.Revolver.Cylinder =
                //     GameManager.Instance.StageManager.CheckPointCylinder;
                // ガンベルトの状態を復元
                // playerController.BulletCountManager.BulletCounts =
                //     GameManager.Instance.StageManager.CheckPointGunBelt;
            }
            _stageStartPerformanceObjct.gameObject.SetActive(false);
        }
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
