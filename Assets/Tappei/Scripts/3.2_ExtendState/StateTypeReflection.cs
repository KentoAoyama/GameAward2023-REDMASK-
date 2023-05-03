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
    private bool _isPostured;

    public StateTypeReflection(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        _delay = _shieldController.ShieldParams.StiffeningTime;
        _shieldController.PlayAnimation(AnimationName.Reflection);
        //Debug.Log("パリィ");
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
        if (_time > _delay - 1.0f && !_isPostured)
        {
            _isPostured = true;
            Controller.PlayAnimation(AnimationName.Posture);
            //Debug.Log("もどし");
        }
        else if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
        }
    }

    protected override void Exit()
    {
        _time = 0;
        _delay = 0;
        _isPostured = false;
        //Debug.Log("パリィおわり");
    }
}
