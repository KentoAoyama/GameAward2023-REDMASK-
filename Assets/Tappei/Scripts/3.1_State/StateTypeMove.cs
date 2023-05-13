using UnityEngine;

/// <summary>
/// プレイヤーに向けて移動する状態のクラス
/// </summary>
public class StateTypeMove : StateTypeBase
{
    /// <summary>
    /// 前フレームと同じ位置にいるので移動をキャンセルするという判定になる閾値
    /// 移動速度によっては値を変更する必要がある
    /// </summary>
    static readonly float MoveCancelThreshold = 0.01f;

    private float _time;
    private int _cachedSEIndex;
    /// <summary>
    /// 移動量が0の状態が一定時間続いたらIdle状態に遷移させるために前フレームでの位置が必要
    /// </summary>
    private Vector3 _prevPos;

    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayer();
        ResetOnEnter();
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
        if (TransitionAtMoveCancel()) return;
    }

    protected override void Exit()
    {
        Controller.CancelMoveToTarget();
        GameManager.Instance.AudioManager.StopSE(_cachedSEIndex);
    }

    public override void OnDisable()
    {
        GameManager.Instance.AudioManager.StopSE(_cachedSEIndex);
    }

    /// <summary>
    /// 余計な値を継承先に公開しなくて良いようにEnter()の処理をまとめておく
    /// </summary>
    protected void ResetOnEnter()
    {
        _prevPos = Vector3.positiveInfinity;
        _time = 0;

        _cachedSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", Controller.Params.RunSEName);
    }

    /// <summary>
    /// 移動がキャンセルされた場合はIdle状態に遷移する
    /// </summary>
    protected virtual bool TransitionAtMoveCancel()
    {
        if (IsMoveCancel())
        {
            TryChangeState(StateType.Idle);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 視界から外れたらIdle状態に、攻撃範囲内に入ったらAttack状態に遷移する
    /// </summary>
    protected virtual bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Attack);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 前フレームからの移動量が0の状態が一定時間続くなら移動がキャンセルされたとみなす
    /// </summary>
    protected bool IsMoveCancel()
    {
        // TODO:毎フレーム呼んでいるので余裕があれば改善する
        float distance = Vector3.Distance(_prevPos, Controller.transform.position);
        if (distance <= MoveCancelThreshold)
        {
            _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        }
        else
        {
            _time = 0;
        }
        _prevPos = Controller.transform.position;

        // 移動量が0の状態が続いた際にIdle状態に遷移させるまでの時間
        float timeThreshold = 0.25f;
        return _time > timeThreshold;
    }
}
