using UnityEngine;

/// <summary>
/// �������p
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    private ShieldEnemyController _shieldController;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        // ���̃X�e�[�g�ɓ��邽�тɍU��������
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        // �U�����邽�߂Ɉړ�����
        // �U�����邽�тɃA�j���[�V�������ŏ�����Đ�����K�v������
        // ���U���ƃA�j���[�V�����̍Đ��^�C�~���O�������Ă��Ȃ��Ƃ�������
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            // �U���͈͂̕������ړ����U������
            _shieldController.Attack();
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return;
        }
    }
}
