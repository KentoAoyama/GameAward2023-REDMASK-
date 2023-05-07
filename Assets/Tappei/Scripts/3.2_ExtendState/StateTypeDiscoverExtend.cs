/// <summary>
/// �������p
/// �v���C���[�������ɉ��o�p�ɑJ�ڂ����Ԃ̃N���X
/// �����ɂ����Move��������Attack��ԂɑJ�ڂ���
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
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
    }

    /// <summary>
    /// �e�𔽎˂�����Reflection��ԂɑJ�ڂ���
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.DiscoverExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��x���������王�E�̊O�ɏo�Ă��܂����ꍇ�ł���xMove��ԂɑJ�ڂ���
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (_isTransitionable)
        {
            if (result == SightResult.InSight || result == SightResult.OutSight)
            {
                TryChangeState(StateType.MoveExtend);
                return true;
            }
            else
            {
                TryChangeState(StateType.AttackExtend);
                return true;
            }
        }

        return false;
    }
}