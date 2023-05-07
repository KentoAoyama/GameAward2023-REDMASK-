using UnityEngine;

/// <summary>
/// ‚‚¿—p
/// ƒvƒŒƒCƒ„[‚ğ’T‚·‚½‚ß‚ÉˆÚ“®‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// ŠÔŒo‰ß‚ÅIdleó‘Ô‚É‘JˆÚ‚·‚é
/// </summary>
public class StateTypeSearchExtend : StateTypeSearch
{
    private ShieldEnemyController _shieldController;

    public StateTypeSearchExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;

        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            _time = 0;
            TryChangeState(StateType.IdleExtend);
        }
    }

    /// <summary>
    /// ’e‚ğ”½Ë‚µ‚½‚çReflectionó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.SearchExtend;
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
}
