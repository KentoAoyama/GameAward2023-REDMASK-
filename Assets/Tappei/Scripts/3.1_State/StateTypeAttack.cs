using UnityEngine;

/// <summary>
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// 遷移を繰り返すことでの連射対策として、この値は状態の遷移をしても初期化されない
    /// </summary>
    protected float _time;
    /// <summary>
    /// 一度攻撃するまで遷移させないためのフラグ
    /// </summary>
    private bool _afterAction;
    /// <summary>
    /// 攻撃処理の呼び出し＆アニメーションの再生を待つためのフラグ
    /// 攻撃の判定が終わったタイミングと同期している
    /// </summary>
    private bool _beforeAnim;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) 
    {
    }

    protected override void Enter()
    {
        // 攻撃までの間、遷移元のアニメーションが再生され続けないように一度Idle状態のアニメーションを再生する
        Controller.PlayAnimation(AnimationName.Idle);
        _time = 0;
        _afterAction = false;
        _beforeAnim = true;
    }

    protected override void Stay()
    {
        Controller.UpdateIdle();

        if (TransitionDefeated()) return;
        AttackAtInterval();
        Controller.DrawGuideline();
        if (Transition()) return;
    }

    /// <summary>
    /// 一定間隔で攻撃する
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate + Controller.Params.AttackDelay + 0.05f)
        {
            _time = 0;
            _afterAction = true;
            _beforeAnim = true;
        }
        if (_time > Controller.Params.AttackRate && _beforeAnim)
        {
            Controller.Attack();
            Controller.PlayAnimation(AnimationName.Attack);
            _beforeAnim = false;
        }
    }

    /// <summary>
    /// 視界から外れたらIdle状態に、攻撃範囲から外れたらMove状態に遷移する
    /// </summary>
    protected virtual bool Transition()
    {
        if (!_afterAction) return false;

        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return true;
        }

        return false;
    }
}
