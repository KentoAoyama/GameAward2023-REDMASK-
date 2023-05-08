using UnityEngine;

/// <summary>
/// StateRegisterクラスから生成するので、継承したステートの
/// コンストラクタはこのステートと同じである必要がある
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

    public StateTypeBase(EnemyController controller, StateType type)
    {
        Controller = controller;
        Type = type;
    }

    protected EnemyController Controller { get; }
    public StateType Type { get; }

    /// <summary>
    /// 1度の呼び出しでステートの段階に応じてEnter() Stay() Exit()のうちどれか1つが実行される
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

    /// <summary>
    /// 撃破された場合の処理はDefeated状態以外は共通処理なので基底クラスに実装してある
    /// 遷移する場合はtrueが返る
    /// </summary>
    protected bool TransitionDefeated()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return true;
        }

        return false;
    }

    public virtual void OnPause() { }
    public virtual void OnResume() { }
    public virtual void OnDisable() { }

    /// <summary>
    /// Enter()が呼ばれてかつ、ステートの遷移処理を呼んでいない場合のみ遷移可能
    /// </summary>
    public bool TryChangeState(StateType type)
    {
        StateTypeBase state = Controller.GetState(type);

        if (_stage == Stage.Enter)
        {
            Debug.LogWarning("Enter()が呼ばれる前にステートを遷移することは不可能: 遷移先: " + state);
            return false;
        }
        else if (_stage == Stage.Exit)
        {
            Debug.LogWarning("既に別のステートに遷移する処理が呼ばれています: 遷移先: " + state);
            return false;
        }

        _stage = Stage.Exit;
        _nextState = state;

        return true;
    }
}