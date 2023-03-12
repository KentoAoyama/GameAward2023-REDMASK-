using UnityEngine;

/// <summary>
/// 敵のステートマシンで使用する各ステートの基底クラス
/// 各ステートは必ずこのクラスを継承する必要がある
/// </summary>
public abstract class StateTypeBase
{
    private enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    private Stage _stage;
    private StateTypeBase _nextState;
    private EnemyStateMachine _stateMachine;

    public StateTypeBase(EnemyStateMachine stateMachine, StateType stateType)
    {
        StateType = stateType;
        _stateMachine = stateMachine;
    }

    public StateType StateType;

    /// <summary>1度の呼び出しでEnter()/Stay()/Exit()のどれか1つが実行される</summary>
    public StateTypeBase Execute()
    {
        if (_stage == Stage.Enter)
        {
            Enter();
            _stage = Stage.Stay;
        }
        else if (_stage == Stage.Stay)
        {
            Stay();
        }
        else if (_stage == Stage.Exit)
        {
            Exit();
            _stage = Stage.Enter;

            return _nextState;
        }

        return this;
    }

    protected virtual void Enter() { }
    protected virtual void Stay() { }
    protected virtual void Exit() { }

    public virtual void Pause() { }
    public virtual void Resume() { }

    /// <summary>
    /// EnemyStateMachineからどのステートに遷移するかが渡される
    /// EnemyStateMachineの次のUpdate()でExit()が呼ばれた後、ステートが切り替わる
    /// Enter()が呼ばれる前にステートを切り替える事は出来ない
    /// 既に遷移処理が呼ばれていた場合はこの遷移処理をキャンセルする
    /// </summary>
    public bool TryChangeState(StateTypeBase state)
    {
        if (_stage == Stage.Enter)
        {
            Debug.LogWarning("Enter()が呼ばれる前にステートを遷移することは不可能: " + state);
            return false;
        }
        else if (_stage == Stage.Exit)
        {
            Debug.LogWarning("既に別のステートに遷移する処理が呼ばれています: " + state);
            return false;
        }

        _stage = Stage.Exit;
        _nextState = state;

        return true;
    }
}