using UnityEngine;

/// <summary>
/// プレイヤーを探すために移動する状態のクラス
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    public StateTypeSearch(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("探す状態のEnter()");
    }

    protected override void Stay()
    {
        Debug.Log("探す状態のStay()");
    }

    protected override void Exit()
    {
        Debug.Log("探す状態のExit()");
    }
}
