/// <summary>
/// 撃破された状態のクラス
/// これ以上は遷移しない
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Death);
    }
}