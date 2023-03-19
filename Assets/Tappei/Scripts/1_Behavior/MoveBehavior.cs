using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// 移動する際に使用するクラス
/// 
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    /// <summary>
    /// 移動先に到着した際にぷるぷるしないようにする為の値
    /// 値を大きくすればより正確に移動先にたどり着くが、速度次第ではぷるぷるしてしまう
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;
    /// <summary>
    /// 毎フレームRayを飛ばさないように、うろうろ出来る移動範囲を更新する間隔を設定する<br></br>
    /// 間隔を狭めればより正確なうろうろが出来る
    /// </summary>
    private static readonly float UpdateWanderingCenterPosInterval = 0.15f;

    [Header("移動方向に向けるオブジェクトの設定")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [Header("移動速度の設定")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 4.0f;

    private Rigidbody2D _rigidbody;
    private WanderingPositionHolder _wanderingPositionHolder;
    private CancellationTokenSource _cts;

    // TODO:デバッグ用の値なのできちんとした値に直す
    private float _debugTimeSpeed = 1;

    private void Awake()
    {
        _wanderingPositionHolder = GetComponent<WanderingPositionHolder>();
        InitRigidbodySettings();
        InitWanderingCenterPos();
    }

    private void OnDisable()
    {
        CancelMoving();
    }

    /// <summary>一定間隔でうろうろ出来る移動範囲を更新する</summary>
    private void InitWanderingCenterPos()
    {
        //this.UpdateAsObservable()
        //    .ThrottleFirst(TimeSpan.FromSeconds(UpdateWanderingCenterPosInterval))
        //    .Subscribe(_ => 
        //    {
        //        _wanderingPositionHolder.SetWanderingCenterPos();
        //    })
        //    .AddTo(this);
    }

    /// <summary>デフォルトから変更した値を設定する</summary>
    private void InitRigidbodySettings()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.angularDrag = 0;
        _rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// Searchステートに遷移した際に外部から呼び出す
    /// うろうろ出来る移動範囲のうち、ランダムな箇所に移動する
    /// </summary>
    public void StartWalkToWanderingTarget()
    {
        Transform target = _wanderingPositionHolder.GetWanderingTarget();
        StartRunToTarget(target);
    }

    /// <summary>歩いて移動先に向かう際に外部から呼び出す</summary>
    public void StartWalkToTarget(Transform target) => StartMoveToTarget(target, _walkSpeed);

    /// <summary>走って移動先に向かう際に外部から呼び出す</summary>
    public void StartRunToTarget(Transform target) => StartMoveToTarget(target, _runSpeed);

    /// <summary>
    /// 現在の移動をキャンセルしてその場に留まる際に外部から呼び出す
    /// また、別の移動先に向かう際はこのメソッドを呼んで現在の移動をキャンセルすること
    /// </summary>
    public void CancelMoving()
    {
        _cts?.Cancel();
        SetVelocityToStop();
    }

    private void SetVelocityToStop()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    private void StartMoveToTarget(Transform target, float moveSpeed)
    {
        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
    }

    /// <summary>
    /// FixedUpdate()のタイミングでターゲットに向かって1フレーム分だけ移動する事によって
    /// ターゲットへの移動を行う<br></br>
    /// 引数がTransformのためターゲットが動いていても追従する
    /// </summary>
    private async UniTask MoveToTargetAsync(Transform target, float moveSpeed)
    {
        _cts.Token.ThrowIfCancellationRequested();

        while (true)
        {
            SetVelocityToTarget(target, moveSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
    }

    private void SetVelocityToTarget(Transform target, float moveSpeed)
    {
        Vector3 targetPos = target.position;
        SetVelocityToTarget(targetPos, moveSpeed);
    }

    /// <summary>
    /// Velocityをターゲットの方向に向けることでターゲットへの移動を行う
    /// スローモーションに対応している
    /// </summary>
    private void SetVelocityToTarget(Vector3 targetPos, float moveSpeed)
    {
        Vector3 velo = targetPos - transform.position;

        if (velo.sqrMagnitude < moveSpeed / ArrivalTolerance)
        {
            velo = Vector3.zero;
        }
        else
        {
            velo = Vector3.Normalize(velo) * moveSpeed;
        }

        TurnToTarget(targetPos);

        velo.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velo * _debugTimeSpeed;
    }

    private void TurnToTarget(Vector3 targetPos)
    {
        float diff = targetPos.x - transform.position.x;
        int dir = (int)Mathf.Sign(diff);

        _spriteTrans.localScale = new Vector3(dir, 1, 1);
        
        Vector3 pos = _eyeTrans.localPosition;
        pos.x = MathF.Abs(pos.x) * dir;
        _eyeTrans.localPosition = pos;

        int angle = dir == 1 ? 0 : 180;
        _eyeTrans.eulerAngles = new Vector3(0, 0, angle);
    }
}
