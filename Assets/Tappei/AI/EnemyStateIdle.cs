using UnityEngine;

/// <summary>
/// �����~�܂��Ă����Ԃ̃N���X
/// </summary>
public class EnemyStateIdle : EnemyStateBase
{
    public EnemyStateIdle(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("�A�C�h����Ԃ�Enter()");
    }
}
