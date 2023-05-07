using UnityEngine;

/// <summary>
/// ƒvƒŒƒCƒ„[‚ğ’T‚·‚½‚ß‚ÉˆÚ“®‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// ŠÔŒo‰ß‚ÅIdleó‘Ô‚É‘JˆÚ‚·‚é
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    protected float _time;
    private int _cachedSEIndex;

    public StateTypeSearch(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Search);
        Controller.MoveSeachForPlayer();

        _cachedSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", Controller.Params.WalkSEName);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    protected override void Exit()
    {
        _time = 0;
        Controller.CancelMoveToTarget();
        GameManager.Instance.AudioManager.StopSE(_cachedSEIndex);
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
    /// ŠÔŒo‰ß‚ÅIdleó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            _time = 0;
            TryChangeState(StateType.Idle);
            return true;
        }

        return false;
    }
}
