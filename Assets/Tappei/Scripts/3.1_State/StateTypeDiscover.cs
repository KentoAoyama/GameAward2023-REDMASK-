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
    private Tween _tween;

    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.PlayDiscoverPerformance();

        float delay = Controller.Params.DiscoverStateTransitionDelay;
        _tween = DOVirtual.DelayedCall(delay, () => _isTransitionable = true);

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Discover");
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
    }

    protected override void Exit()
    {
        _tween.Kill();
        _isTransitionable = false;
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
                TryChangeState(StateType.Move);
                return true;
            }
            else
            {
                TryChangeState(StateType.Attack);
                return true;
            }
        }

        return false;
    }
}
