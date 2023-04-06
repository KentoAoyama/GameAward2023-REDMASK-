using UnityEngine;

/// <summary>
/// �������p
/// �v���C���[��T�����߂Ɉړ������Ԃ̃N���X
/// ���Ԍo�߂�Idle��ԂɑJ�ڂ���
/// </summary>
public class StateTypeSearchExtend : StateTypeSearch
{
    private ShieldEnemyController _shieldController;

    public StateTypeSearchExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
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
            _shieldController.LastStateType = StateType.SearchExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return;
        }

        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            _time = 0;
            TryChangeState(StateType.IdleExtend);
        }
    }
}
