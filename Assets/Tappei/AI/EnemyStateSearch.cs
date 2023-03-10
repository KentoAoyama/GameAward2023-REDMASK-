using UnityEngine;

/// <summary>
/// プレイヤーを探すために移動する状態のクラス
/// </summary>
public class EnemyStateSearch : EnemyStateBase
{
    public EnemyStateSearch(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("探す状態のEnter()");
    }
}
