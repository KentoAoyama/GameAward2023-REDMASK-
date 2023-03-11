using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステートマシンで使用する各ステートを登録しておくクラス
/// </summary>
public class EnemyStateRegister
{
    private EnemyStateMachine _stateMachine;
    private EnemyStateMachineHelper _stateMachineHelper;
    private Dictionary<EnemyStateType, StateTypeBase> _stateDic = new();

    public EnemyStateRegister(EnemyStateMachine stateMachine, EnemyStateMachineHelper helper)
    {
        _stateMachine = stateMachine;
        _stateMachineHelper = helper;
    }

    /// <summary>
    /// 取りうるステートの種類を指定して生成＆辞書型に登録する
    /// 登録したステートはGetState()によって取得可能
    /// </summary>
    public void Register(EnemyStateType type)
    {
        StateTypeBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(EnemyStateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassTypeWithEnum(type);
        object[] args = { _stateMachine };
        StateTypeBase instance = (StateTypeBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public StateTypeBase GetState(EnemyStateType type)
    {
        if (_stateDic.TryGetValue(type, out StateTypeBase state))
        {
            return state;
        }
        else
        {
            Debug.LogWarning("対応するステートが登録されていません: " + type);
            return null;
        }
    }
}
