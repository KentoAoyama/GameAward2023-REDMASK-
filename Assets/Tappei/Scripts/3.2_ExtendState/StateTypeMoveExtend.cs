/// <summary>
/// �������p
/// �v���C���[�Ɍ����Ĉړ������Ԃ̃N���X
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
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.DiscoverExtend;
            TryChangeState(StateType.Reflection);
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