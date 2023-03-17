/// <summary>
/// 立ち止まっているステートのクラス
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    public StateTypeIdle(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _messenger.SendMessage(BehaviorType.StopMove);
    }
}
