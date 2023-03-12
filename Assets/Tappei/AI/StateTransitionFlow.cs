using System;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitionFlow : MonoBehaviour
{
    [Serializable]
    private struct TransitionFlow
    {
        [SerializeField] private StateType _currentState;
        [SerializeField] private StateType _nextstate;
        [SerializeField] private StateTransitionTrigger _trigger;

        public StateType CurrentState => _currentState;
        public StateType Nextstate => _nextstate;
        public StateTransitionTrigger Trigger => _trigger;
    }

    [Header("‘JˆÚŒ³/‘JˆÚæ/‘JˆÚğŒ")]
    [SerializeField] private TransitionFlow[] _transitionFlow;

    private Dictionary<(StateType, StateTransitionTrigger), StateType> _transitionDic;

    private void Awake()
    {
        _transitionDic = new(_transitionFlow.Length);
        foreach (TransitionFlow flow in _transitionFlow)
        {
            _transitionDic.Add((flow.CurrentState, flow.Trigger), flow.Nextstate);
        }
    }

    public StateType GetNextState(StateType currentState, StateTransitionTrigger trigger)
    {
        if (_transitionDic.TryGetValue((currentState, trigger), out StateType nextState))
        {
            return nextState;
        }
        else
        {
            Debug.LogError("‘JˆÚæ‚ª“o˜^‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ: " + currentState + " " + trigger);
            return StateType.Idle;
        }
    }
}
