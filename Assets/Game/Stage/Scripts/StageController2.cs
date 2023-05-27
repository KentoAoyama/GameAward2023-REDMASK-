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
    [SerializeField]
    private StageFadeIn _stageFadeIn = default;
    [SerializeField]
    private StageFadeOut _stageFadeOut = default;

    private void Start()
    {
        GameManager.Instance.AudioManager.Load();
        GameManager.Instance.AudioManager.PlayBGM("CueSheet_Gun", "BGM_Ingame");

        _stageFadeIn.FadeIn();
    }
    /// <summary>
    /// 復活
    /// </summary>
    public async void Revival(StageStartMode stageStartMode)
    {
        GameManager.Instance.StageManager.StageStartMode = stageStartMode;
        GameManager.Instance.PauseManager.ClearCount();
        if (stageStartMode == StageStartMode.FromTheBeginning)
        {
            await _stageFadeOut.FadeOut();
            SceneManager.LoadScene(_firstStageSceneName);
            GameManager.Instance.StageManager.Clear();
        }
        else if (stageStartMode == StageStartMode.JustBefore)
        {
            await _stageFadeOut.FadeOut();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}