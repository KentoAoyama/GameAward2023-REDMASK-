// 日本語対応
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleStartButton : MonoBehaviour
{
    [SerializeField]
    private StageType _stageOne = default;
    [SceneName, SerializeField]
    private string _prepareScene = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        GameManager.Instance.PauseManager.ClearCount();
        DOTween.KillAll();
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Start");
        GameManager.Instance.CompletedStageManager.Clear(); // ステージのクリア状況をリセットする
        GameManager.Instance.StageSelectManager.SetStage(_stageOne); // これから赴くステージを設定する
        SceneManager.LoadScene(_prepareScene); // 準備シーンに遷移する
    }
}
