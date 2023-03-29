using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盾持ち用
/// 盾に攻撃を受けてしばらく硬直している状態
/// </summary>
public class StateTypeReflection : StateTypeBase
{
    public StateTypeReflection(EnemyController controller, StateType stateType)
    : base(controller, stateType) { }
}
