// 日本語対応
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    private StageType _stageType = default;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SetGoToStage);
    }
    /// <summary>
    /// 向かうステージを選択する
    /// </summary>
    public void SetGoToStage()
    {
        // シーン遷移に伴い、ポーズカウントを初期化、
        GameManager.Instance.PauseManager.ClearCount();
        GameManager.Instance.StageSelectManager.SetStage(_stageType);
    }
}
