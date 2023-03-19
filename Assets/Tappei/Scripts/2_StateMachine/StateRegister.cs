using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このクラスを用いて使用するステートの生成と登録を行う
/// </summary>
public class StateRegister
{
    /// <summary>
    /// StateTypeに1:1で対応したステートを検索するので
    /// ステートの数だけ初期容量を確保しておく
    /// </summary>
    private static readonly int StateDicCap = Enum.GetValues(typeof(StateType)).Length;

    private Dictionary<StateType, StateTypeBase> _stateDic = new(StateDicCap);

    /// <summary>
    /// 取りうるステートの種類を指定して生成＆辞書型に登録する
    /// 登録したステートはGetState()によって取得可能
    /// </summary>
    public void Register(StateType type, object stateArg)
    {
        if (_stateDic.ContainsKey(type))
        {
            Debug.LogWarning("辞書にステートが既に登録されています: " + type);
            return;
        }

        StateTypeBase state = CreateInstance(type, stateArg);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(StateType type, object stateArg)
    {
        Type stateClass = GetStateClassType(type);

        if (stateClass == null)
        {
            Debug.LogError("StateTypeにステートが紐づけられていません: " + type);;
            return null;
        }

        // ステートのコンストラクタの引数の順に並べている
        object[] args = { stateArg, type };
        StateTypeBase instance = (StateTypeBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public StateTypeBase GetState(StateType type)
    {
        if (_stateDic.TryGetValue(type, out StateTypeBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("対応するステートが辞書に登録されていません: " + type);
            return null;
        }
    }

    /// <summary>
    /// 列挙型に対応したステートのクラスの型を返すので
    /// 新しくステートを作った際には、この処理の分岐に追加して列挙型とクラスを紐づける必要がある
    /// </summary>
    private Type GetStateClassType(StateType type)
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
