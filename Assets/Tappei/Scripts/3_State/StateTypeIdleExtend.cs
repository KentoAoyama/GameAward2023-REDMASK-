using UnityEngine;

/// <summary>
/// 盾持ち用
/// 立ち止まっている状態のクラス
/// 時間経過でSearch状態に遷移する
/// </summary>
public class StateTypeIdleExtend : StateTypeIdle
{
    public StateTypeIdleExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) { }

    protected override void Enter()
    {

    }
}
