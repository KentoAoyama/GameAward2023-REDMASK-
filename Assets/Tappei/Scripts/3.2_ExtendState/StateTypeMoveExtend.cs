/// <summary>
/// 盾持ち用
/// プレイヤーに向けて移動する状態のクラス
/// </summary>
public class StateTypeMoveExtend : StateTypeMove
{
    private ShieldEnemyController _shieldController;

    public StateTypeMoveExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.DiscoverExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        if (IsMoveCancel())
        {
            TryChangeState(StateType.SearchExtend);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.SearchExtend);
            return;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.AttackExtend);
            return;
        }
    }
}
