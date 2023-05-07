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

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage_Shield");
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (RecoverProcess()) return;
    }

    protected override void Exit()
    {
        _time = 0;
        _delay = 0;
        _isPostured = false;
        _shieldController.RecoverShield();
    }

    /// <summary>
    /// 時間経過で盾を構え直すアニメーションを再生する
    /// アニメーションの再生後、最後の状態に遷移する
    /// </summary>
    private bool RecoverProcess()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay - 1.0f && !_isPostured)
        {
            _isPostured = true;
            Controller.PlayAnimation(AnimationName.Posture);
        }
        else if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
            return true;
        }

        return false;
    }
}
