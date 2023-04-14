// 日本語対応
using UniRx;

public class StageSelectManager
{
    private ReactiveProperty<StageType> _goToStage = new ReactiveProperty<StageType>(StageType.One);

    public IReadOnlyReactiveProperty<StageType> GoToStageType => _goToStage;

    public void SetStage(StageType stageType)
    {
        _goToStage.Value = stageType;
    }
}

[System.Serializable]
public enum StageType
{
    NotSet,
    Test,
    One,
    Two,
    Three,
    Four,
    Test2,
}