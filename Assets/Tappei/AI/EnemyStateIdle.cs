using UnityEngine;

/// <summary>
/// 立ち止まっている状態のクラス
/// </summary>
public class EnemyStateIdle : EnemyStateBase
{
    public EnemyStateIdle(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("アイドル状態のEnter()");
    }
}
