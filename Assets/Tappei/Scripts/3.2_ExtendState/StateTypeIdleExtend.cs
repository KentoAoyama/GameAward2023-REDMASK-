using UnityEngine;

/// <summary>
/// �������p
/// �����~�܂��Ă����Ԃ̃N���X
/// ���Ԍo�߂�Search��ԂɑJ�ڂ���
/// </summary>
public class StateTypeIdleExtend : StateTypeIdle
{
    private ShieldEnemyController _shieldController;

    public StateTypeIdleExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        _shieldController.Idle();

        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.IdleExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale * Time.deltaTime;
        if (_time > _delay)
        {
            TryChangeState(StateType.SearchExtend);
            return;
        }
    }
}
