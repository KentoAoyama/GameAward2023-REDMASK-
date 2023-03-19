using System;
using UnityEngine;

/// <summary>
/// ステートマシンで使用する汎用的な処理をまとめたヘルパークラス
/// </summary>
public class StateMachineHelper
{
    /// <summary>
    /// 列挙型に対応したステートのクラスの型を返すので
    /// 新しくステートを作った際には、この処理の分岐に追加して列挙型とクラスを紐づける必要がある
    /// </summary>
    public Type GetStateClassType(StateType type)
    {
        switch (type)
        {
            case StateType.Idle: return typeof(StateTypeIdle);
            case StateType.Search: return typeof(StateTypeSearch);
            case StateType.Attack: return typeof(StateTypeAttack);
            case StateType.Defeated: return typeof(StateTypeDefeated);
            case StateType.Move: return typeof(StateTypeMove);
            default:
                Debug.LogError("対応するステートが紐づけられていません: " + type);
                return null;
        }
    }
}
