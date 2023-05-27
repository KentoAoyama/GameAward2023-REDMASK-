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

    [Header("ステージクリア用")]
    [SerializeField]
    private bool _isFinal = false;
    [SerializeField]
    private int _currentStageNumberForFinal = 0;

    [SerializeField]
    private StageFadeOut _stageFadeOut = default;

    bool isPlaying = false;
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと接触したとき実行する。
        if (collision.tag == _playerTag)
        {
            if (isPlaying) return;
            isPlaying = true;
            GameManager.Instance.StageManager.StageStartMode = StageStartMode.JustBefore;
            // 弾の残り数を保存する。
            // （敗北時に直前からやり直すボタンを選択した場合にその値を使用する。）
            var player = GameObject.FindGameObjectWithTag(_playerTag);
            var playerController = player.GetComponent<PlayerController>();
            GameManager.Instance.StageManager.SetCheckPointBulletsCount(
                playerController.Revolver.Cylinder, playerController.BulletCountManager.BulletCounts);
            GameManager.Instance.StageManager.CylinderIndex = playerController.Revolver.CurrentChamber;

            await _stageFadeOut.FadeOut();

            // シーンを更新する。
            SceneManager.LoadScene(_nextSceneName);

            if (_isFinal)
            {
                GameManager.Instance.CompletedStageManager.SetCompletedStage(_currentStageNumberForFinal);
            }
        }
    }
}
