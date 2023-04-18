// 日本語対応
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// 復活
    /// </summary>
    public void Revival(StageStartMode stageStartMode)
    {
        GameManager.Instance.StageManager.StageStartMode = stageStartMode;
        GameManager.Instance.PauseManager.ClearCount();
        if (stageStartMode == StageStartMode.FromTheBeginning)
        {
            SceneManager.LoadScene(_firstStageSceneName);
            GameManager.Instance.StageManager.Clear();
        }
        else if (stageStartMode == StageStartMode.JustBefore)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}