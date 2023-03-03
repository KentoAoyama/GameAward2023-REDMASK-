// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    private StageType _stageType = default;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetGoToStage);
    }
    /// <summary>
    /// 向かうステージを選択する
    /// </summary>
    public void SetGoToStage()
    {
        GameManager.Instance.StageSelectManager.SetStage(_stageType);
    }
}
