using UnityEngine;

/// <summary>
/// �v���C���[��T�����߂Ɉړ������Ԃ̃N���X
/// </summary>
public class EnemyStateSearch : EnemyStateBase
{
    public EnemyStateSearch(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("�T����Ԃ�Enter()");
    }
}
