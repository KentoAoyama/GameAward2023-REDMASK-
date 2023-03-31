using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������p
/// ���ɍU�����󂯂Ă��΂炭�d�����Ă�����
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
