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
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
        if (TransitionAtMoveCancel()) return;
    }

    /// <summary>
    /// 弾を反射したらReflection状態に遷移する
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.MoveExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 視界から外れたらIdle状態に、攻撃範囲内に入ったらAttack状態に遷移する
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return true;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.AttackExtend);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 移動がキャンセルされた場合はIdle状態に遷移する
    /// </summary>
    private bool TransitionAtMoveCancel()
    {
        if (IsMoveCancel())
        {
            TryChangeState(StateType.IdleExtend);
            return true;
        }

        return false;
    }
}
