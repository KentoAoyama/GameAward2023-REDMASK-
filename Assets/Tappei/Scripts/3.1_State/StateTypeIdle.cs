using UnityEngine;

/// <summary>
/// �����~�܂��Ă����Ԃ̃N���X
/// ���Ԍo�߂�Search��ԂɑJ�ڂ���
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    protected float _delay;
    protected float _time;

    public StateTypeIdle(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Idle);

        // �����_���Ȏ��ԂőJ�ڂ���悤�ɐݒ肷��
        _delay = Controller.Params.GetRandomIdleStateTimer();
    }

    protected override void Stay()
    {
        Controller.UpdateIdle();

        if (TransitionDefeated()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    protected override void Exit()
    {
        _time = 0;
    }

    /// <summary>
    /// ���E��/�U���͈͓��ɓ�������Discover��ԂɑJ�ڂ���
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���Ԍo�߂�Idle��������Search��ԂɑJ�ڂ���
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.Idle : StateType.Search);
            return true;
        }

        return false;
    }
}
