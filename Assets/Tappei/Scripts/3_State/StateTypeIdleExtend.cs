using UnityEngine;

/// <summary>
/// �������p
/// �����~�܂��Ă����Ԃ̃N���X
/// ���Ԍo�߂�Search��ԂɑJ�ڂ���
/// </summary>
public class StateTypeIdleExtend : StateTypeIdle
{
    public StateTypeIdleExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) { }

    protected override void Enter()
    {

    }
}
