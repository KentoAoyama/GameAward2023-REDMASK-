// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージ選択画面用のシーントランジションボタン
/// </summary>
public class SceneTransitionButtonForStageSelect : MonoBehaviour
{
    [Tooltip("このボタンを押下した時に遷移するシーンを選択してください。")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    [SerializeField]
    private KnifeAnimaiton _knife = default;
    [SerializeField]
    private Image _fadeImage = default;
    [SerializeField]
    private float _fadeTime = 1f;
    [SerializeField]
    private Image _disableClicksImage = default;
    [SerializeField]
    private EventSystem _eventSystem = default;

    [SerializeField]
    private GameObject _knifeParent = default;

    private Button _button = default;
    private Image _image = default;

    public GameObject KnifeParent => _knifeParent;
    public Button Button => _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnSceneChange);
    }
    /// <summary>
    /// ボタンを押下したときに呼び出すことを想定して作成したメソッド。<br/>
    /// シーンを変更する。
    /// </summary>
    public async void OnSceneChange()
    {
        GameManager.Instance.PauseManager.ClearCount();
        _disableClicksImage.gameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(null);
        await _knife.Play(async () =>
        {
            _fadeImage.gameObject.SetActive(true);
            await UniTask.Delay(800);
            await _fadeImage.DOFade(1f, _fadeTime).OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(_nextSceneName);
            });
        });
    }
}
