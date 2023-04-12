using UnityEngine;

/// <summary>
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>�J�ڂ���܂ł̊Ԋu�𒲐����邽�߂Ɏg�p����l</summary>
    protected static readonly int AttackTimerMag = 60;
    /// <summary>
    /// �J�ڂ��J��Ԃ����Ƃł̘A�ˑ΍�Ƃ���
    /// ���̒l�͏�Ԃ̑J�ڂ����Ă�����������Ȃ�
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

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Search);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return;
        }
    }
}
