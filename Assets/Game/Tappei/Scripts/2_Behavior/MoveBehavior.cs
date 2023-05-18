using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// 各状態において移動する際に使用するクラス
/// 必要な各Moduleのクラスを制御する
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    private Transform _transform;
    private CancellationTokenSource _cts;

    [SerializeField] private TurnModule _turnModule;
    [SerializeField] private DetectorModule _detectorModule;
    [SerializeField] private RigidBodyModule _rigidbodyModule;
    [SerializeField] private WaypointModule _waypointModule;
    [Tooltip("Spriteの左右に応じた処理をしたいので参照が必要")]
    [SerializeField] private Transform _sprite;

    /// <summary>
    /// Pause()が呼ばれるとtrueにResume()が呼ばれるとfalseになる
    /// </summary>
    private bool _isPause;

    /// <summary>
    /// Spriteの左右の向きに合わせた処理をする際に使う
    /// 右向き: 1 左向き: -1
    /// </summary>
    public int SpriteDir => (int)Mathf.Sign(_sprite.localScale.x);

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _waypointModule.InitOnAwake();

        FootPosUpdateStart();
    }

    private void OnDisable()
    {
        CancelMoveToTarget();
    }

    public void OnPause()
    {
        _isPause = true;
        _rigidbodyModule.SaveVelocity();
    }

    public void OnResume()
    {
        _isPause = false;
        _rigidbodyModule.LoadVelocity();
    }

    /// <summary>
    /// 一定間隔で足元の座標を更新する
    /// この処理は他のクラスやTimeScaleに影響されないので
    /// Update()内のメソッドだがこのクラス内で実行している
    /// </summary>
    private void FootPosUpdateStart()
    {
        // 毎フレームRayを飛ばさないように、Search状態での移動範囲を更新する間隔を設定する
        float updateFootPosInterval = 0.15f;

        this.UpdateAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(updateFootPosInterval))
            .Subscribe(_ => 
            {
                if (_detectorModule.DetectFootPos(_transform.position, out Vector3 hitPos))
                {
                    _waypointModule.UpdateFootPos(hitPos);
                }
            })
            .AddTo(this);
    }

    /// <summary>
    /// キャラクターを左向きに配置する際に使用する
    /// </summary>
    public void TurnLeft()
    {
        Vector3 dir = _transform.position;
        dir.x += -int.MaxValue;
        _turnModule.TurnTowardsTarget(dir, _transform);
    }

    public void StartMoveSearchForPlayer(float moveSpeed, float distance, bool useRandomDistance)
    {
        Transform waypoint = _waypointModule.GetSearchWaypoint(distance, useRandomDistance);
        StartMoveToTarget(waypoint, moveSpeed);
    }

    public void StartMoveForward(float distance, float moveSpeed)
    {
        Transform forwardWaypoint = _waypointModule.GetForwardWaypoint(SpriteDir * distance);
        StartMoveToTarget(forwardWaypoint, moveSpeed);
    }

    /// <summary>
    /// ターゲットに向けて移動する
    /// 基本はこのメソッドを呼ぶことでターゲットを追いかける
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
    }

    public void StartMoveToTarget(Vector3 pos, float moveSpeed)
    {
        _cts = new CancellationTokenSource();
        MoveToTargetAsync(pos, moveSpeed).Forget();
    }

    private async UniTaskVoid MoveToTargetAsync(Transform target, float moveSpeed)
    {
        _cts.Token.ThrowIfCancellationRequested();

        _rigidbodyModule.UpdateKinematic(false);
        _turnModule.TurnTowardsTarget(target.position, _transform);
        while (_detectorModule.DetectFloorInFront(SpriteDir, _transform))
        {
            if (!_isPause)
            {
                _rigidbodyModule.SetVelocityToTarget(target.position, moveSpeed, _transform);
                _turnModule.TurnTowardsTarget(target.position, _transform);
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
        CancelMoveToTarget();
    }

    /// <summary>
    /// MoveToTargetAsync()のオーバーライド
    /// 引数がTransformからVector3に変わっただけ
    /// </summary>
    private async UniTaskVoid MoveToTargetAsync(Vector3 pos, float moveSpeed)
    {
        _cts.Token.ThrowIfCancellationRequested();

        _rigidbodyModule.UpdateKinematic(false);
        _turnModule.TurnTowardsTarget(pos, _transform);
        while (_detectorModule.DetectFloorInFront(SpriteDir, _transform))
        {
            if (!_isPause)
            {
                _rigidbodyModule.SetVelocityToTarget(pos, moveSpeed, _transform);
                _turnModule.TurnTowardsTarget(pos, _transform);
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
        CancelMoveToTarget();
    }

    /// <summary>
    /// 現在の移動をキャンセルしてその場に留まる
    /// 別の移動先に向かう際はこのメソッドを呼んで現在の移動をキャンセルすること
    /// </summary>
    public void CancelMoveToTarget()
    {
        _cts?.Cancel();
        _rigidbodyModule.SetFallVelocity();
    }

    /// <summary>
    /// 足元からのRayがヒットしない場合はそのまま落下し
    /// ヒットした場合はPositionをその座標にすることで滑らないようにしている
    /// </summary>
    public void Idle()
    {
        if (_isPause) return;

        bool onGround = _detectorModule.DetectOnGroundIdle(_transform.position, out Vector3 hitPos);
        _rigidbodyModule.UpdateKinematic(onGround);
    }
}
