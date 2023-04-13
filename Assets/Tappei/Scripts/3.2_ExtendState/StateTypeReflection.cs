using UnityEngine;

/// <summary>
/// 盾持ち用
/// 盾に攻撃を受けてしばらく硬直している状態
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
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale * Time.deltaTime;
        if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
        }
    }
}
