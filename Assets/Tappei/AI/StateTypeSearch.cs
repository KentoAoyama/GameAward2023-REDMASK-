using UnityEngine;

/// <summary>
/// ƒvƒŒƒCƒ„[‚ğ’T‚·‚½‚ß‚ÉˆÚ“®‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    public StateTypeSearch(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    protected override void Enter()
    {
        Debug.Log("’T‚·ó‘Ô‚ÌEnter()");
    }

    protected override void Stay()
    {
        Debug.Log("’T‚·ó‘Ô‚ÌStay()");
    }

    protected override void Exit()
    {
        Debug.Log("’T‚·ó‘Ô‚ÌExit()");
    }
}
