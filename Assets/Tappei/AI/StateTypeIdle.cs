using UnityEngine;
using UniRx;
using System;

/// <summary>
/// �����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    public StateTypeIdle(EnemyStateMachine stateMachine, StateType stateType)
        : base(stateMachine, stateType) { }

    protected override void Enter()
    {
        Debug.Log("�A�C�h����Ԃ�Enter()");
    }

    protected override void Stay()
    {
        Debug.Log("�A�C�h����Ԃ�Stay()");
    }

    protected override void Exit()
    {
        Debug.Log("�A�C�h����Ԃ�Exit()");
    }
}
