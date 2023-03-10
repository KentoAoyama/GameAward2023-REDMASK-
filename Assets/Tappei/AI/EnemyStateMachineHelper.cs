using System;
using UnityEngine;

/// <summary>
/// 敵のステートマシンで取りうるステートの種類
/// 各ステートは必ずこの列挙型のうちのどれかに対応していなければならない
/// </summary>
public enum EnemyStateType
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
/// 汎用的な処理をまとめたクラス
/// </summary>
public class EnemyStateMachineHelper
{
    /// <summary>
    /// 列挙型に対応したステートのクラスの型を返すので
    /// 新しくステートを作った際には、この処理の分岐に追加して列挙型とクラスを紐づける必要がある
    /// </summary>
    public Type GetStateClassTypeWithEnum(EnemyStateType type)
    {
        switch (type)
        {
            case EnemyStateType.Idle: return typeof(EnemyStateIdle);
            case EnemyStateType.Search: return typeof(EnemyStateSearch);
            default:
                Debug.LogError("対応するステートが紐づけられていません: " + type);
                return null;
        }
    }
}
