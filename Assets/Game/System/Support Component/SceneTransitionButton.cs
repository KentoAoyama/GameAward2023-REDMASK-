// 日本語対応
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionButton : MonoBehaviour
{
    [Tooltip("このボタンを押下した時に遷移するシーンを選択してください。")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    [SerializeField]
    private StageFadeOut _stageFadeOut = default;

    private void Awake()
    {
        GetComponent<Button>()?.onClick.AddListener(OnSceneChange);
    }
    /// <summary>
    /// ボタンを押下したときに呼び出すことを想定して作成したメソッド。<br/>
    /// シーンを変更する。
    /// </summary>
    public async void OnSceneChange()
    {
        if (_stageFadeOut != null)
        {
            await _stageFadeOut.FadeOut();
        }
        GameManager.Instance.PauseManager.ClearCount();
        DOTween.KillAll();
        SceneManager.LoadScene(_nextSceneName);
    }
}
