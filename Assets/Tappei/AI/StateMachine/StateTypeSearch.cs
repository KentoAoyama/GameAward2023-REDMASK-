using UnityEngine;

/// <summary>
/// �v���C���[��T�����߂Ɉړ�����X�e�[�g�̃N���X
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    public StateTypeSearch(EnemyStateMachine stateMachine, StateType stateType)
        : base(stateMachine, stateType) { }

    protected override void Enter()
    {
        Debug.Log("�T����Ԃ�Enter()");
    }

    protected override void Stay()
    {
        Debug.Log("�T����Ԃ�Stay()");
    }

    protected override void Exit()
    {
        Debug.Log("�T����Ԃ�Exit()");
    }
}