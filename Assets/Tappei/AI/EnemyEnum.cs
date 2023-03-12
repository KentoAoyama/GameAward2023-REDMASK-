/// <summary>
/// 敵のステートマシンで取りうるステートの種類
/// 各ステートは必ずこの列挙型のうちのどれかに対応していなければならない
/// </summary>
public enum StateType
{
    Base, // 各ステートの基底クラス用
    Idle,
    Search,
    Move,
    Attack,
    Damage,
    Death,
    Reflection,
}

/// <summary>
/// ステート遷移のトリガーとなるイベントの列挙型
/// この列挙型を含むメッセージの送受信に使用する
/// </summary>
public enum StateTransitionTrigger
{
    TimeElapsed,
    PlayerFind,
    PlayerHide,
    PlayerInAttackRange,
    PlayerOutAttackRange,
}