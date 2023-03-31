using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ‚‚¿—p
/// ‚‚ÉUŒ‚‚ğó‚¯‚Ä‚µ‚Î‚ç‚­d’¼‚µ‚Ä‚¢‚éó‘Ô
/// </summary>
public class StateTypeReflection : StateTypeBase
{
    private ShieldEnemyController _shieldController;
    private float _delay;
    private float _time;

    public StateTypeReflection(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        _delay = _shieldController.ShieldParams.StiffeningTime;
    }

    protected override void Stay()
    {
        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale * Time.deltaTime;
        if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
        }
    }
}
