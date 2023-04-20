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

    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayer();

        _prevPos = Vector3.one * -1000;
        _timer = 0;
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (IsMoveCancel())
        {
            TryChangeState(StateType.Idle);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Attack);
            return;
        }
    }

    protected override void Exit()
    {
        Controller.CancelMoving();
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

        return _timer > Controller.Params.MoveCancelTimerThreshold;
    }
}
