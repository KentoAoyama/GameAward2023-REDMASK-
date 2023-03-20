using UniRx;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵のステートマシン
/// </summary>
//[RequireComponent(typeof(StateTransitionFlow))]
public class EnemyStateMachine : MonoBehaviour/*, IPausable*/
{
    //private ReactiveProperty<StateTypeBase> _currentState = new();
    //private StateRegister _stateRegister = new();

    //private void Awake()
    //{
    //    _behaviorFacade = new(gameObject, _enemyParamsSO);
    //    InitStateRegister();
    //    InitCurrentState();
    //}

    //private void Update()
    //{
    //    _currentState.Value = _currentState.Value.Execute();
    //}

    //private void InitStateRegister()
    //{
    //    _stateRegister.Register(StateType.Idle, _behaviorFacade);
    //    _stateRegister.Register(StateType.Search, _behaviorFacade);
    //    _stateRegister.Register(StateType.Move, _behaviorFacade);
    //    _stateRegister.Register(StateType.Attack, _behaviorFacade);
    //    _stateRegister.Register(StateType.Defeated, _behaviorFacade);
    //}

    //private void InitCurrentState()
    //{
    //    StateType state = _enemyParamsSO.EntryState;
    //    _currentState.Value = _stateRegister.GetState(state);
    //}

    //public void Pause()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void Resume()
    //{
    //    throw new System.NotImplementedException();
    //}
}