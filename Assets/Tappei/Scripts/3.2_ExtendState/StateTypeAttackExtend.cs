using UnityEngine;

/// <summary>
/// �������p
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    /// <summary>
    /// �ԍ������l�߂Ă��鎞�Ԃ��v�Z���邽�߂ɍU���͈͂ɒ萔��������
    /// </summary>
    private static readonly float MovingDistanceMag = 15.0f;
    /// <summary>
    /// ���̍U���܂ł̑҂����Ԃ͍U�����[�V�������l�����ă}�C�i�X�̒l��ݒ肷��
    /// </summary>
    private static readonly float NextAttackDelay = -80.0f;

    private ShieldEnemyController _shieldController;
    /// <summary>
    /// �ԍ������l�߂Ă��铮�쒆���ǂ����̃t���O
    /// </summary>
    private bool _isApproaching;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        _time = Controller.Params.AttackRate;
        _shieldController.PlayAnimation(AnimationName.Idle);
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

        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate && !_isApproaching)
        {
            _isApproaching = true;
            _shieldController.MoveForward();
            _shieldController.PlayAnimation(AnimationName.Move);
        }
        if(_time > Controller.Params.AttackRate + MovingDistanceMag * Controller.Params.AttackRange)
        {
            _time = NextAttackDelay;
            _isApproaching = false;
            _shieldController.CancelMoving();
            _shieldController.Attack();
            _shieldController.PlayAnimation(AnimationName.Attack);
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

    protected override void Exit()
    {
        _time = 0;
        _isApproaching = false;
    }
}
