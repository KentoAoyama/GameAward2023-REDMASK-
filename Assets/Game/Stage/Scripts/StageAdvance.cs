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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと接触したとき実行する。
        if (collision.tag == _playerTag)
        {
            GameManager.Instance.StageManager.StageStartMode = StageStartMode.JustBefore;
            // 弾の残り数を保存する。
            // （敗北時に直前からやり直すボタンを選択した場合にその値を使用する。）
            var player = GameObject.FindGameObjectWithTag(_playerTag);
            var playerController = player.GetComponent<PlayerController>();
            GameManager.Instance.StageManager.SetCheckPointBulletsCount(
                playerController.Revolver.Cylinder, playerController.BulletCountManager.BulletCounts);
            GameManager.Instance.StageManager.CylinderIndex = playerController.Revolver.CurrentChamber;

            // シーンを更新する。
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
