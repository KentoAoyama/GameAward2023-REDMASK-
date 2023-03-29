using UnityEngine;

/// <summary>
/// 盾持ち用
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) { }
}
