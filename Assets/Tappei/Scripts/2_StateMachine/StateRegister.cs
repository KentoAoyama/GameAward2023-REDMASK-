using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステートマシンで使用する各ステートを登録しておくクラス
/// ステートマシンが自身の初期化時にこのクラスを用いて使用するステートの生成と登録を行う
/// </summary>
public class StateRegister
{
    /// <summary>
    /// StateTypeに1:1で対応したステートを検索するので
    /// ステートの数だけ初期容量を確保しておく
    /// </summary>
    private static readonly int StateDicCap = Enum.GetValues(typeof(StateType)).Length;

    private BehaviorMessenger _messenger;
    private StateMachineHelper _stateMachineHelper;
    private Dictionary<StateType, StateTypeBase> _stateDic = new(StateDicCap);

    public StateRegister(BehaviorMessenger messenger, StateMachineHelper helper)
    {
        _messenger = messenger;
        _stateMachineHelper = helper;
    }

    /// <summary>
    /// 取りうるステートの種類を指定して生成＆辞書型に登録する
    /// 登録したステートはGetState()によって取得可能
    /// </summary>
    public void Register(StateType type)
    {
        if (_stateDic.ContainsKey(type))
        {
            Debug.LogWarning("辞書にステートが既に登録されています: " + type);
            return;
        }

        StateTypeBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(StateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassType(type);

        if (stateClass == null)
        {
            Debug.LogError("StateTypeにステートが紐づけられていません: " + type);;
            return null;
        }

        object[] args = { _messenger, type };
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
}
