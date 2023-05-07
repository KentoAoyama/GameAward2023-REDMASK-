using UnityEngine;

/// <summary>
/// ‚‚¿—p
/// —§‚¿~‚Ü‚Á‚Ä‚¢‚éó‘Ô‚ÌƒNƒ‰ƒX
/// ŠÔŒo‰ß‚ÅSearchó‘Ô‚É‘JˆÚ‚·‚é
/// </summary>
public class StateTypeIdleExtend : StateTypeIdle
{
    private ShieldEnemyController _shieldController;

    public StateTypeIdleExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        _shieldController.UpdateIdle();

        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    /// <summary>
    /// ’e‚ğ”½Ë‚µ‚½‚çReflectionó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.IdleExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ‹ŠE“à/UŒ‚”ÍˆÍ“à‚É“ü‚Á‚½‚çDiscoveró‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ‹ŠE“à/UŒ‚”ÍˆÍ“à‚É“ü‚Á‚½‚çDiscoveró‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.IdleExtend : StateType.SearchExtend);
            return true;
        }

        return false;
    }
}
