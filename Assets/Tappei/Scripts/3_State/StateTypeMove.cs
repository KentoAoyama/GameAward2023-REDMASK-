/// <summary>
/// プレイヤーに向けて移動するステートのクラス
/// </summary>
public class StateTypeMove : StateTypeBase
{
    public StateTypeMove(BehaviorFacade facade, StateType stateType)
        : base(facade, stateType) { }

    protected override void Enter()
    {
        //Facade.SendMessage(BehaviorType.MoveToPlayer);
    }

    protected override void Exit()
    {
        //Facade.SendMessage(BehaviorType.StopMove);
    }
}
