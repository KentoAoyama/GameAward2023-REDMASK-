using UnityEngine;

/// <summary>
/// 敵のステートマシンで使用する各ステートの基底クラス
/// 各ステートは必ずこのクラスを継承する必要がある
/// </summary>
public abstract class EnemyStateBase
{
    private enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    private Stage _stage;
    private EnemyStateBase _nextState;
    private EnemyStateMachine _stateMachine;

    public EnemyStateBase(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// ステートの遷移処理を呼ぶと、次のEnemyStateMachineのUpdate()のタイミングから
    /// 次のステートに切り替わる
    /// </summary>
    public EnemyStateBase Update()
    {
        if (_stage == Stage.Enter)
        {
            Enter();
            _stage = Stage.Stay;
        }
        if (_stage == Stage.Stay)
        {
            Stay();
        }
        if (_stage == Stage.Exit)
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

    private void ChangeState(EnemyStateType type)
    {
        _stage = Stage.Exit;
    }

    /// <summary>
    /// 任意のステートに遷移する処理だが
    /// 既に遷移処理が呼ばれていた場合はこの遷移処理をキャンセルする
    /// </summary>
    protected bool TryChangeState(EnemyStateType type)
    {
        if (_stage == Stage.Stay)
        {
            ChangeState(type);
            return true;
        }
        else
        {
            Debug.LogWarning("既に別のステートに遷移する処理が呼ばれています: " + type);
            return false;
        }
    }
}
