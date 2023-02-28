// 日本語対応
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionButton : MonoBehaviour
{
    [Tooltip("このボタンを押下した時に遷移するシーンを選択してください。")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    private void Start()
    {
        GetComponent<Button>()?.onClick.AddListener(OnSceneChange);
    }
    /// <summary>
    /// ボタンを押下したときに呼び出すことを想定して作成したメソッド。<br/>
    /// シーンを変更する。
    /// </summary>
    public void OnSceneChange()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
