using UnityEngine;
using UniRx;
using System;

/// <summary>
/// �����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    public StateTypeIdle(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("�A�C�h����Ԃ�Enter()");
    }

    protected override void Exit()
    {
        Debug.Log("�A�C�h����Ԃ�Exit()");
    }
}
