/// <summary>
/// プレイヤーに向けて移動するステートのクラス
/// </summary>
public class StateTypeMove : StateTypeBase
{
    public StateTypeMove(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _messenger.SendMessage(BehaviorType.MoveToPlayer);
    }

    protected override void Exit()
    {
        _messenger.SendMessage(BehaviorType.StopMove);
    }
}
