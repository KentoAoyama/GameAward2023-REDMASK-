// 日本語対応
using UnityEngine;

public class PrepareSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _testCanvas = default;
    [SerializeField]
    private GameObject _stage1Canvas = default;
    [SerializeField]
    private GameObject _stage2Canvas = default;
    [SerializeField]
    private GameObject _stage3Canvas = default;
    [SerializeField]
    private GameObject _stage4Canvas = default;

    private void Awake()
    {
        // 準備画面用BGM再生
        GameManager.Instance.AudioManager.PlayBGM("CueSheet_Gun", "BGM_Stage_Selection");

        _testCanvas.SetActive(false);
        _stage1Canvas.SetActive(false);
        _stage2Canvas.SetActive(false);
        _stage3Canvas.SetActive(false);
        _stage4Canvas.SetActive(false);

        switch (GameManager.Instance.StageSelectManager.GoToStageType.Value)
        {
            case StageType.One: _stage1Canvas.SetActive(true); break;
            case StageType.Two: _stage2Canvas.SetActive(true); break;
            case StageType.Three: _stage3Canvas.SetActive(true); break;
            case StageType.Four: _stage4Canvas.SetActive(true); break;
            default: _testCanvas.SetActive(true); break;
        }
    }
}
