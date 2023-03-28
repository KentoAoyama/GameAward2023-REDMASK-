/// <summary>
/// �v���C���[�������ɉ��o�p�ɑJ�ڂ����Ԃ̃N���X
/// �����ɂ����Move��������Attack��ԂɑJ�ڂ���
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.DiscoverPerformance();
    }

    protected override void Stay()
    {
        // ��x���������王�E�̊O�ɏo�Ă��܂����ꍇ�ł���xMove��ԂɑJ�ڂ���
        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.OutSight)
        {
            TryChangeState(StateType.Move);
        }
        else
        {
            TryChangeState(StateType.Attack);
        }
    }
}
