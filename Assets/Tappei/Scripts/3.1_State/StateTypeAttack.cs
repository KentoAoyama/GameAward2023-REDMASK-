using UnityEngine;

/// <summary>
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// �U������܂ł̃^�C�}�[�̃J�E���g�̔{��
    /// deltaTime�Ƃ̏�Z�ōU���܂ł̊Ԋu�𒲐�����
    /// </summary>
    protected static readonly int AttackTimerMag = 60;
    /// <summary>
    /// �J�ڂ��J��Ԃ����Ƃł̘A�ˑ΍�Ƃ���
    /// StateTypeAttack�N���X�ł͂��̒l�͏�Ԃ̑J�ڂ����Ă�����������Ȃ�
    /// </summary>
    protected float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);
        //Controller.MoveToPlayer();
    }

    protected override void Stay()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            Controller.Attack();
        }

        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return;
        }
    }
}
