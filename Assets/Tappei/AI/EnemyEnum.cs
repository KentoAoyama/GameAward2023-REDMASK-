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
    Defeated,
    Reflection,
}

/// <summary>
/// ステート遷移のトリガーとなるイベントの種類
/// この列挙型を含むメッセージを受け取ったらステートが遷移する
/// </summary>
public enum StateTransitionTrigger
{
    TimeElapsed,
    PlayerFind,
    PlayerHide,
    PlayerInAttackRange,
    PlayerOutAttackRange,
}

/// <summary>
/// 取りうる行動の種類
/// 各ステートが対応した行動の処理を呼び出すのに使用する
/// </summary>
public enum BehaviorType
{
    Attack,
    SearchMove,
    StopMove,
    Defeated,
}