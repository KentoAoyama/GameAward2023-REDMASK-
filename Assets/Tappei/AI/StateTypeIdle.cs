using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 立ち止まっている状態のクラス
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    public StateTypeIdle(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("アイドル状態のEnter()");
    }

    protected override void Exit()
    {
        Debug.Log("アイドル状態のExit()");
    }
}
