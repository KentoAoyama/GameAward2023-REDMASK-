/// <summary>
/// 撃破された際のステートのクラス
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _messenger.SendMessage(BehaviorType.Defeated);
    }
}
