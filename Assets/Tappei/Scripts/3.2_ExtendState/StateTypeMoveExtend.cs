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
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
        if (TransitionAtMoveCancel()) return;
    }

    /// <summary>
    /// �e�𔽎˂�����Reflection��ԂɑJ�ڂ���
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
    /// ���E����O�ꂽ��Idle��ԂɁA�U���͈͓��ɓ�������Attack��ԂɑJ�ڂ���
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
    /// �ړ����L�����Z�����ꂽ�ꍇ��Idle��ԂɑJ�ڂ���
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
