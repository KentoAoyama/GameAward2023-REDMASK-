// 日本語対応
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトルの続きからボタン
/// </summary>
public class TitleContinueButton : MonoBehaviour
{
    [SceneName, SerializeField]
    private string _stageSelectButton = default;

    public void Awake()
    {
        if (GameManager.Instance.CompletedStageManager.
            LoadStageCompleteNumber() < 0)
        {
            //Destroy(gameObject);
            //return;
        } // どのステージもクリアしていない状態であれば続きからボタンを破棄する。

        GetComponent<Button>().onClick.AddListener(OnSceneChange);
    }
    /// <summary>
    /// ボタンを押下したときに呼び出すことを想定して作成したメソッド。<br/>
    /// シーンを変更する。
    /// </summary>
    public void OnSceneChange()
    {
        GameManager.Instance.PauseManager.ClearCount();
        DOTween.KillAll();
        SceneManager.LoadScene(_stageSelectButton);
    }
}
