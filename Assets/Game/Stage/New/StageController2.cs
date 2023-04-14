// 日本語対応
using Player;
using UnityEngine;

/// <summary>
/// ステージの制御を行うコンポーネント
/// </summary>
public class StageController2 : MonoBehaviour
{
    [Header("プレイヤーのタグ")]
    [TagName, SerializeField]
    private string _playerTag = default;
    [Header("最初からやり直すを選択した場合に読み込むシーンの名前")]
    [SceneName, SerializeField]
    private string _firstStageSceneName = default;
    [Header("次のシーンの名前")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    private void Awake()
    {
        // シーン開始時に現在の弾の数を保存する
        // （敗北時に直前からやり直すボタンを選択した場合にその値を使用する。）
        var player = GameObject.FindGameObjectWithTag(_playerTag);
        var playerController = player.GetComponent<PlayerController>();
        GameManager.Instance.StageManager.SetBulletsCount(
            playerController.Revolver.Cylinder, playerController.BulletCountManager.BulletCounts);
    }
}