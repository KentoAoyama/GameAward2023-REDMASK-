using DG.Tweening;

/// <summary>
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    /// <summary>
    /// 発見時のアニメーションの終了を待って遷移させるためのフラグ
    /// このフラグ立つまで遷移は不可能だが、視界は機能している
    /// </summary>
    bool _isTransitionable;
    Tween _tween;

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.DiscoverPerformance();

        float delay = Controller.Params.DiscoverStateTransitionDelay;
        _tween = DOVirtual.DelayedCall(delay, () => _isTransitionable = true);
    }

    protected override void Stay()
    {
        // 一度発見したら視界の外に出てしまった場合でも一度Move状態に遷移する
        SightResult result = Controller.IsFindPlayer();
        if (_isTransitionable)
        {
            if (result == SightResult.InSight || result == SightResult.OutSight)
            {
                TryChangeState(StateType.Move);
            }
            else
            {
                TryChangeState(StateType.Attack);
            }
        }
    }

    protected override void Exit()
    {
        _tween.Kill();
        _isTransitionable = false;
    }
}
