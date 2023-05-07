using UnityEngine;

/// <summary>
/// ‚‚¿—p
/// ˆê’èŠÔŠu‚ÅUŒ‚‚ğ‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    /// <summary>
    /// ŠÔ‡‚¢‚ğ‹l‚ß‚Ä‚­‚éŠÔ‚ğŒvZ‚·‚é‚½‚ß‚ÉUŒ‚”ÍˆÍ‚É’è”‚ğ‚©‚¯‚é
    /// </summary>
    private static readonly float MovingDistanceMag = 15.0f;
    /// <summary>
    /// Ÿ‚ÌUŒ‚‚Ü‚Å‚Ì‘Ò‚¿ŠÔ‚ÍUŒ‚ƒ‚[ƒVƒ‡ƒ“‚ğl—¶‚µ‚Äƒ}ƒCƒiƒX‚Ì’l‚ğİ’è‚·‚é
    /// </summary>
    private static readonly float NextAttackDelay = -80.0f;

    private ShieldEnemyController _shieldController;

    /// <summary>
    /// Œp³Œ³‚Å‚ ‚éStateTypeAttackƒNƒ‰ƒX‚Æ‚ÍUŒ‚‚Ì‹““®‚ªˆá‚¤‚Ì‚Å•Ê‚Ì•Ï”‚ğéŒ¾‚·‚é
    /// ‚±‚¿‚ç‚Íó‘Ô‚Ì‘JˆÚ‚Å‰Šú‰»‚³‚ê‚é
    /// </summary>
    private float _time;
    /// <summary>
    /// ŠÔ‡‚¢‚ğ‹l‚ß‚Ä‚­‚é“®ì’†‚©‚Ç‚¤‚©‚Ìƒtƒ‰ƒO
    /// </summary>
    private bool _isApproaching;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        // TODO:UŒ‚‚Ìˆ—‚ª•¡G‚È‚Ì‚Å‚Ü‚¾‰˜‚¢
        _time = Controller.Params.AttackRate;
        _shieldController.PlayAnimation(AnimationName.Idle);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        AttackAtInterval();
        if (Transition()) return;
    }

    protected override void Exit()
    {
        _time = 0;
        _isApproaching = false;
    }

    /// <summary>
    /// ŠÔŒo‰ß‚Å‘O•û‚ÉˆÚ“®->UŒ‚‚ğs‚¤
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate && !_isApproaching)
        {
            _isApproaching = true;
            _shieldController.MoveForward();
            _shieldController.PlayAnimation(AnimationName.Move);
        }
        if (_time > Controller.Params.AttackRate + MovingDistanceMag * Controller.Params.AttackRange)
        {
            _time = NextAttackDelay;
            _isApproaching = false;
            _shieldController.CancelMoveToTarget();
            _shieldController.Attack();
            _shieldController.PlayAnimation(AnimationName.Attack);
        }
    }

    /// <summary>
    /// ’e‚ğ”½Ë‚µ‚½‚çReflectionó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ‹ŠE‚©‚çŠO‚ê‚½‚çIdleó‘Ô‚ÉAUŒ‚”ÍˆÍ‚©‚çŠO‚ê‚½‚çMoveó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return true;
        }

        return false;
    }
}
