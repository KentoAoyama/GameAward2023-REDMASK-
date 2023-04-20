using UnityEngine;

/// <summary>
/// 盾持ち用
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    private ShieldEnemyController _shieldController;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        // このステートに入るたびに攻撃をする
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        // 攻撃するために移動する
        // 攻撃するたびにアニメーションを最初から再生する必要がある
        // ↑攻撃とアニメーションの再生タイミングが合っていないとおかしい
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            // 攻撃範囲の分だけ移動しつつ攻撃する
            _shieldController.Attack();
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return;
        }
    }
}
