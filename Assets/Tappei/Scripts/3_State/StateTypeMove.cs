/// <summary>
/// プレイヤーに向けて移動する状態のクラス
/// </summary>
public class StateTypeMove : StateTypeBase
{
    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.MoveToPlayer();
    }

    protected override void Stay()
    {
        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Search);
            return;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Attack);
            return;
        }
    }

    protected override void Exit()
    {
        Controller.CancelMoving();
    }
}
