using System;
using UnityEngine;

/// <summary>
/// 汎用的な処理をまとめたクラス
/// </summary>
public class EnemyStateMachineHelper
{
    /// <summary>
    /// 列挙型に対応したステートのクラスの型を返すので
    /// 新しくステートを作った際には、この処理の分岐に追加して列挙型とクラスを紐づける必要がある
    /// </summary>
    public Type GetStateClassTypeWithEnum(StateType type)
    {
        switch (type)
        {
            case StateType.Idle: return typeof(StateTypeIdle);
            case StateType.Search: return typeof(StateTypeSearch);
            default:
                Debug.LogError("対応するステートが紐づけられていません: " + type);
                return null;
        }
    }
}
