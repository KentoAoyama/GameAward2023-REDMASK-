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
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;

        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            _time = 0;
            TryChangeState(StateType.IdleExtend);
        }
    }

    /// <summary>
    /// �e�𔽎˂�����Reflection��ԂɑJ�ڂ���
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.SearchExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���E��/�U���͈͓��ɓ�������Discover��ԂɑJ�ڂ���
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return true;
        }

        return false;
    }
}
