// 日本語対応
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class GoToStageButton : MonoBehaviour
{
    [SerializeField]
    private StageTypeStageNamePair[] _stageTypeStageNamePair = default;
    [SerializeField]
    private MaskSwitch _maskSwitch = default;
    [SerializeField]
    private Color _disableColor = Color.white;
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;

    private Image _image = null;

    private void Start()
    {
        _image = GetComponent<Image>();
        var button = GetComponent<Button>();
        _maskSwitch.IsSet.Subscribe(value =>
        {
            button.enabled = value;
            _image.color = value ? Color.white : _disableColor;
        });
        button.onClick.AddListener(GoToStage);
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
        _bulletPrepareControl?.AssignBulletsCount();
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
