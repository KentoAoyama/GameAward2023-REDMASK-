/// <summary>
/// �v���C���[��T�����߂Ɉړ������Ԃ̃N���X
/// ���Ԍo�߂�Idle��ԂɑJ�ڂ���
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    /// <summary>�J�ڂ���܂ł̊Ԋu�𒲐����邽�߂Ɏg�p����l</summary>
    private static readonly int TransitionTimerMag = 60; 

    protected float _interval;
    protected float _time;

    public StateTypeSearch(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Search);

        // �ړ����s�����\�b�h���Ăяo���Ď��Ԍo�߂�Idle�ɑJ�ڂ���
        // ���J��Ԃ��Ď��͂�T��������
        _interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed * TransitionTimerMag;
        Controller.SearchMoving();
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale;
        if (_time > _interval)
        {
            _time = 0;
            TryChangeState(StateType.Idle);
        }
    }

    protected override void Exit()
    {
        _time = 0;
        Controller.CancelMoving();
    }
}
