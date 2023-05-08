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
        : base(controller, stateType) 
    {
        _time = controller.Params.AttackRate;
    }

    protected override void Enter()
    {
        // �U���܂ł̊ԁA�J�ڌ��̃A�j���[�V�������Đ����ꑱ���Ȃ��悤�Ɉ�xIdle��Ԃ̃A�j���[�V�������Đ�����
        Controller.PlayAnimation(AnimationName.Idle);
    }

    protected override void Stay()
    {
        Controller.UpdateIdle();

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
            Controller.PlayAnimation(AnimationName.Attack);
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
