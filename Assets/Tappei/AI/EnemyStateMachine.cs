using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 敵のステートマシン
/// </summary>
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    private ReactiveProperty<EnemyStateBase> _currentState = new();
    private EnemyStateMachineStateRegister _stateRegister;

    public IReadOnlyReactiveProperty<EnemyStateBase> CurrentState;

    private void Awake()
    {
        EnemyStateMachineHelper helper = new();
        _stateRegister = new(this, helper);
        _stateRegister.Register(EnemyStateType.Idle);
        _stateRegister.Register(EnemyStateType.Search);

        _currentState.Value = _stateRegister.GetState(EnemyStateType.Search);
    }

    void Start()
    {
        // 状態遷移に使うもの、条件遷移のみを書いて、遷移するかを返すクラスを用意する
        // プレイヤーを発見した
        // プレイヤーを見失った
        // ダメージを受けた
        // 体力が一定以下になった
        // 一定時間がたった
    }

    void Update()
    {
        _currentState.Value.Update();
    }



    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}