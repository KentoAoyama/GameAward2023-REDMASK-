// 日本語対応
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToStageButton : MonoBehaviour
{
    [SerializeField]
    private StageTypeStageNamePair[] _stageTypeStageNamePair = default;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(GoToStage);
    }

    public void GoToStage()
    {
        GameManager.Instance.StageManager.StageStartMode = StageStartMode.FromTheBeginning;

        for (int i = 0; i < _stageTypeStageNamePair.Length; i++)
        {
            if (GameManager.Instance.StageSelectManager.GoToStageType.Value ==
                _stageTypeStageNamePair[i].Type)
            {
                SceneManager.LoadScene(_stageTypeStageNamePair[i].SceneName);
                GameManager.Instance.StageSelectManager.SetStage(StageType.NotSet);
                break;
            }
        }
    }
}
[Serializable]
public struct StageTypeStageNamePair
{
    [SerializeField]
    private StageType _type;
    [SerializeField]
    private string _sceneName;

    public StageType Type => _type;
    public string SceneName => _sceneName;
}
