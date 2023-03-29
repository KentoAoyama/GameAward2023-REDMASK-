using UnityEngine;

/// <summary>
/// 盾持ち用
/// プレイヤー発見時に演出用に遷移する状態のクラス
/// 距離によってMoveもしくはAttack状態に遷移する
/// </summary>
public class StateTypeDiscoverExtend : StateTypeDiscover
{
    public StateTypeDiscoverExtend(EnemyController controller, StateType type)
        : base(controller, type) { }
}
