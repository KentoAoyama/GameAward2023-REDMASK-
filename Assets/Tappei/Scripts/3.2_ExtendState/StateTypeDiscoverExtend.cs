/// <summary>
/// 盾持ち用
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscoverExtend : StateTypeDiscover
{
    private ShieldEnemyController _shieldController;

    public StateTypeDiscoverExtend(EnemyController controller, StateType type)
        : base(controller, type)
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

        // 一度発見したら視界の外に出てしまった場合でも一度Move状態に遷移する
        SightResult result = Controller.LookForPlayerInSight();
        if (_isTransitionable)
        {
            if (result == SightResult.InSight || result == SightResult.OutSight)
            {
                TryChangeState(StateType.MoveExtend);
            }
            else
            {
                TryChangeState(StateType.AttackExtend);
            }

            return;
        }
    }
}