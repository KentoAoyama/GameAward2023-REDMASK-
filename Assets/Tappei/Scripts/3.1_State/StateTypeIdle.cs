using UnityEngine;

/// <summary>
/// —§‚¿~‚Ü‚Á‚Ä‚¢‚éó‘Ô‚ÌƒNƒ‰ƒX
/// ŠÔŒo‰ß‚ÅSearchó‘Ô‚É‘JˆÚ‚·‚é
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    protected float _delay;
    protected float _time;

    public StateTypeIdle(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Idle);

        // ƒ‰ƒ“ƒ_ƒ€‚ÈŠÔ‚Å‘JˆÚ‚·‚é‚æ‚¤‚Éİ’è‚·‚é
        _delay = Controller.Params.GetRandomIdleStateTimer();
    }

    protected override void Stay()
    {
        Controller.UpdateIdle();

        if (TransitionDefeated()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    protected override void Exit()
    {
        _time = 0;
    }

    /// <summary>
    /// ‹ŠE“à/UŒ‚”ÍˆÍ“à‚É“ü‚Á‚½‚çDiscoveró‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ŠÔŒo‰ß‚ÅIdle‚à‚µ‚­‚ÍSearchó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.Idle : StateType.Search);
            return true;
        }

        return false;
    }
}
