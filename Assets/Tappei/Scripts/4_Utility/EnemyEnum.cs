/// <summary>
/// ステートの種類の判定を行う際に使用する列挙型
/// 各ステートは必ずこの列挙型のうちのどれかに対応していなければならない
/// </summary>
public enum StateType
{
    Unknown,
    Idle,
    Search,
    Discover,
    Move,
    Attack,
    Defeated,
    Reflection,
}

/// <summary>
/// 視界に対してプレイヤーがどの位置にいるかの判定に使用される列挙型
/// 視界の処理とその結果を受けての分岐に使用される
/// </summary>
public enum SightResult
{
    OutSight,
    InSight,
    InAttackRange,
}