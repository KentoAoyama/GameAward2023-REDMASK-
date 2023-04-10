/// <summary>
/// Œ‚”j‚³‚ê‚½ó‘Ô‚ÌƒNƒ‰ƒX
/// ‚±‚êˆÈã‚Í‘JˆÚ‚µ‚È‚¢
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Dead);
    }
}