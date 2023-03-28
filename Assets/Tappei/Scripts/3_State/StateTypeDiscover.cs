/// <summary>
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscover : StateTypeBase
{
    public StateTypeDiscover(EnemyController controller, StateType type) 
        : base(controller, type) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Discover);
        Controller.DiscoverPerformance();
    }

    protected override void Stay()
    {
        // 一度発見したら視界の外に出てしまった場合でも一度Move状態に遷移する
        SightResult result = Controller.IsFindPlayer();
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
