using DG.Tweening;

/// <summary>
/// �v���C���[�������ɉ��o�p�ɑJ�ڂ����Ԃ̃N���X
/// �����ɂ����Move��������Attack��ԂɑJ�ڂ���
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    /// <summary>
    /// �������̃A�j���[�V�����̏I����҂��đJ�ڂ����邽�߂̃t���O
    /// ���̃t���O�����܂őJ�ڂ͕s�\�����A���E�͋@�\���Ă���
    /// </summary>
    protected bool _isTransitionable;
    Tween _tween;

    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.DiscoverPerformance();

        float delay = Controller.Params.DiscoverStateTransitionDelay;
        _tween = DOVirtual.DelayedCall(delay, () => _isTransitionable = true);
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        // ��x���������王�E�̊O�ɏo�Ă��܂����ꍇ�ł���xMove��ԂɑJ�ڂ���
        SightResult result = Controller.IsFindPlayer();
        if (_isTransitionable)
        {
            if (result == SightResult.InSight || result == SightResult.OutSight)
            {
                TryChangeState(StateType.Move);
            }
            else
            {
                TryChangeState(StateType.Attack);
            }

            return;
        }
    }

    protected override void Exit()
    {
        _tween.Kill();
        _isTransitionable = false;
    }
}
