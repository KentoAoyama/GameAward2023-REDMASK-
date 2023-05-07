using DG.Tweening;

/// <summary>
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    /// <summary>
    /// 発見時のアニメーションの終了を待って遷移させるためのフラグ
    /// このフラグが立つまで遷移は不可能だが、視界は機能している
    /// </summary>
    protected bool _isTransitionable;
    private Tween _tween;

    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.PlayDiscoverPerformance();

        float delay = Controller.Params.DiscoverStateTransitionDelay;
        _tween = DOVirtual.DelayedCall(delay, () => _isTransitionable = true);

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Discover");
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
    }

    protected override void Exit()
    {
        _tween.Kill();
        _isTransitionable = false;
    }

    /// <summary>
    /// 一度発見したら視界の外に出てしまった場合でも一度Move状態に遷移する
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (_isTransitionable)
        {
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
