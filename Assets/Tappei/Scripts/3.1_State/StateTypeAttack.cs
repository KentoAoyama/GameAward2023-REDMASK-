using UnityEngine;

/// <summary>
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// �J�ڂ��J��Ԃ����Ƃł̘A�ˑ΍�Ƃ��āA���̒l�͏�Ԃ̑J�ڂ����Ă�����������Ȃ�
    /// </summary>
    private float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        AttackAtInterval();
        if (Transition()) return;
    }

    /// <summary>
    /// ���Ԋu�ōU������
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            Controller.Attack();
        }
    }

    /// <summary>
    /// ���E����O�ꂽ��Idle��ԂɁA�U���͈͂���O�ꂽ��Move��ԂɑJ�ڂ���
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return true;
        }

        return false;
    }
}
