using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineStateRegister
{
    private EnemyStateMachine _stateMachine;
    private EnemyStateMachineHelper _stateMachineHelper;
    private Dictionary<EnemyStateType, EnemyStateBase> _stateDic = new();

    public EnemyStateMachineStateRegister(EnemyStateMachine stateMachine, EnemyStateMachineHelper helper)
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
        EnemyStateBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private EnemyStateBase CreateInstance(EnemyStateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassTypeWithEnum(type);
        object[] args = { _stateMachine };
        EnemyStateBase instance = (EnemyStateBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public EnemyStateBase GetState(EnemyStateType type)
    {
        if (_stateDic.TryGetValue(type, out EnemyStateBase state))
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
