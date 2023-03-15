using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 移動周りの処理を制御するクラス
/// </summary>
[RequireComponent(typeof(WanderingPositionHolder))]
public class MoveController : MonoBehaviour
{
    /// <summary>
    /// 移動先に到着した際にぷるぷるしないようにする為の値
    /// 値を大きくすればより正確に移動先にたどり着くが、速度次第ではぷるぷるしてしまう
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;

    [SerializeField] private Rigidbody2D _rigidbody;
    [Header("移動速度の設定")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 4.0f;

    private WanderingPositionHolder _wanderingPositionHolder;
    private CancellationTokenSource _cts;

    // TODO:デバッグ用の値なのできちんとした値に直す
    private float _debugTimeSpeed = 1;

    private void Awake()
    {
        _wanderingPositionHolder = GetComponent<WanderingPositionHolder>();
        InitRigidbodySettings();
    }

    // テスト用の呼び出し
    public void TestMove()
    {
        CancelMoving();
        Transform target = _wanderingPositionHolder.GetWanderingTarget();
        StartRunToTarget(target);
    }

    private void OnDisable()
    {
        CancelMoving();
    }

    /// <summary>
    /// デフォルトから変更した値を設定する
    /// インスペクターでの変更忘れ防止
    /// </summary>
    private void InitRigidbodySettings()
    {
        _rigidbody.angularDrag = 0;
        _rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// 歩いて移動先に向かう際に外部から呼び出す
    /// 別の移動先に向かう際はCancelMoving()を呼んで現在の移動をキャンセルすること
    /// </summary>
    public void StartWalkToTarget(Transform target) => StartMoveToTarget(target, _walkSpeed);
    /// <summary>
    /// 走って移動先に向かう際に外部から呼び出す
    /// 別の移動先に向かう際はCancelMoving()を呼んで現在の移動をキャンセルすること
    /// </summary>
    public void StartRunToTarget(Transform target) => StartMoveToTarget(target, _runSpeed);

    /// <summary>
    /// 現在の移動をキャンセルしてその場に留まる際に外部から呼び出す
    /// 別の移動先に向かう際はこのメソッドを呼んで現在の移動をキャンセルすること
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
    private async UniTaskVoid MoveToTargetAsync(Transform target, float moveSpeed)
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

        velo.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velo * _debugTimeSpeed;
    }
}
