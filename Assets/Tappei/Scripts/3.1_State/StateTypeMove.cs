using UnityEngine;

/// <summary>
/// プレイヤーに向けて移動する状態のクラス
/// </summary>
public class StateTypeMove : StateTypeBase
{
    // 以下2つは地面の端などで移動がキャンセルされた場合に
    // 一定時間後に遷移させる処理に必要な変数
    private Vector3 _prevPos;
    private float _timer;
    private int _cachedSEIndex;

    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayer();

        _prevPos = Vector3.one * -1000;
        _timer = 0;

        _cachedSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", Controller.Params.RunSEName);
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

    /// <summary>
    /// 移動がキャンセルされた場合はIdle状態に遷移する
    /// </summary>
    private bool TransitionAtMoveCancel()
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
    private bool Transition()
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
        float distance = Vector3.Distance(_prevPos, Controller.transform.position);
        if (distance <= Mathf.Epsilon)
        {
            _timer += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        }
        else
        {
            _timer = 0;
        }
        _prevPos = Controller.transform.position;

        return _timer > EnemyParamsSO.MoveCancelTimeThreshold;
    }
}
