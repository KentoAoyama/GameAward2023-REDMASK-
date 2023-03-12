using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ステートの遷移を登録するクラス
/// ステートマシンはこのクラスを用いてどのステートに遷移すれば良いのかを知る
/// </summary>
public class StateTransitionFlow : MonoBehaviour
{
    /// <summary>
    /// インスペクター上で各ステートの遷移を割り当てるための構造体
    /// </summary>
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

    [Header("遷移元/遷移先/遷移条件")]
    [SerializeField] private TransitionFlow[] _transitionFlow;

    private Dictionary<(StateType, StateTransitionTrigger), StateType> _transitionDic;

    private void Awake()
    {
        InitCreateDic();
    }

    private void InitCreateDic()
    {
        _transitionDic = new(_transitionFlow.Length);
        foreach (TransitionFlow flow in _transitionFlow)
        {
            _transitionDic.Add((flow.CurrentState, flow.Trigger), flow.Nextstate);
        }
    }

    /// <summary>
    /// ステートと遷移条件に対応した遷移先のStateTypeを取得する
    /// StateTypeであってステートを取得しているわけではないので注意
    /// </summary>
    public StateType GetNextStateType(StateType current, StateTransitionTrigger trigger)
    {
        if (_transitionDic.TryGetValue((current, trigger), out StateType next))
        {
            return next;
        }
        else
        {
            Debug.LogError("遷移先が登録されていません: " + current + " " + trigger);
            return StateType.Idle;
        }
    }
}
