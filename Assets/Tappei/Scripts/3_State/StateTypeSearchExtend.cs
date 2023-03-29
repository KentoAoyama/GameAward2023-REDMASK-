using UnityEngine;

/// <summary>
/// 盾持ち用
/// プレイヤーを探すために移動する状態のクラス
/// 時間経過でIdle状態に遷移する
/// </summary>
public class StateTypeSearchExtend : StateTypeSearch
{
    public StateTypeSearchExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) { }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
