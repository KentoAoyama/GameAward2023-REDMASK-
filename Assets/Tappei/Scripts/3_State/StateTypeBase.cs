using UnityEngine;

/// <summary>
/// 各ステートは必ずこのクラスを継承する必要がある
/// StateRegisterクラスから生成するので、継承したステートのコンストラクタは
/// このステートと同じである必要がある
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

    public StateTypeBase(BehaviorFacade facade, StateType type)
    {
        Facade = facade;
        Type = type;
    }

    protected BehaviorFacade Facade { get; }
    public StateType Type { get; }

    /// <summary>
    /// 1度の呼び出しでステートの状態に応じて
    /// 遷移した際の処理、そのステート中の処理、抜ける際の処理のうちどれか1つが実行される
    /// </summary>
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
    /// このメソッドを呼んだ場合、ステートマシンで次に現在のステートのExecute()を呼んだ際
    /// Exit()が実行された後、ステートが切り替わる
    /// </summary>
    public bool TryChangeState(StateTypeBase state)
    {
        if (_stage == Stage.Enter)
        {
            Debug.LogWarning("Enter()が呼ばれる前にステートを遷移することは不可能: 遷移先: " + state);
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