using UnityEngine;

/// <summary>
/// プレイヤーを探すために移動するステートのクラス
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    public StateTypeSearch(EnemyStateMachine stateMachine, StateType stateType)
        : base(stateMachine, stateType) { }

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
