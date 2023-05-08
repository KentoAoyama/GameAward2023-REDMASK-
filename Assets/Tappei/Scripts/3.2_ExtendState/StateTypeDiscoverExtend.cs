using UnityEngine;

/// <summary>
/// 盾持ち用
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscoverExtend : StateTypeDiscover
{
    private ShieldEnemyController _shieldController;

    public StateTypeDiscoverExtend(EnemyController controller, StateType type)
        : base(controller, type)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
    }

    /// <summary>
    /// 弾を反射したらReflection状態に遷移する
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.DiscoverExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 一度発見したら視界の外に出てしまった場合でも一度Move状態に遷移する
    /// </summary>
    private bool Transition()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.DiscoverStateTransitionDelay)
        {
            SightResult result = Controller.LookForPlayerInSight();
            if (result == SightResult.InSight || result == SightResult.OutSight)
            {
                TryChangeState(StateType.MoveExtend);
                return true;
            }
            else
            {
                TryChangeState(StateType.AttackExtend);
                return true;
            }
        }

        return false;
    }
}