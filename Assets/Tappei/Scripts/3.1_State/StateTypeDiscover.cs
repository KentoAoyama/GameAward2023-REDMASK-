using UnityEngine;

/// <summary>
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    protected float _time;

    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.PlayDiscoverPerformance();

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Discover");
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
    }

    protected override void Exit()
    {
        _time = 0;
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
                TryChangeState(StateType.Move);
                return true;
            }
            else
            {
                TryChangeState(StateType.Attack);
                return true;
            }
        }

        return false;
    }
}
