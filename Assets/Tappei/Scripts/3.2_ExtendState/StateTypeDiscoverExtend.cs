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
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.DiscoverExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        // ��x���������王�E�̊O�ɏo�Ă��܂����ꍇ�ł���xMove��ԂɑJ�ڂ���
        SightResult result = Controller.IsFindPlayer();
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