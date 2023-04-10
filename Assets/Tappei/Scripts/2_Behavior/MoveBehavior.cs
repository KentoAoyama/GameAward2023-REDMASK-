using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

/// <summary>
/// 各状態において移動する際に使用するクラス
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    /// <summary>
    /// 移動先に到着した際にぷるぷるしないようにする為の値
    /// 値を大きくすればより正確に移動先にたどり着くが、速度次第ではぷるぷるしてしまう
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;
    /// <summary>
    /// 毎フレームRayを飛ばさないように、Search状態での移動範囲を更新する間隔を設定する
    /// </summary>
    private static readonly float UpdateFootPosInterval = 0.15f;

    private static readonly float FootPosRayDistance = 1.0f;
    private static readonly float FloorRayDistance = 6.0f;
    private static readonly float EnemyTypeRayDistance = 1.1f;
    private static readonly Vector2 FootPosRayOffset = new Vector2(0, 0.5f);
    private static readonly Vector2 FloorRayOffset = new Vector2(0, 1.5f);

    [Header("移動方向に向けるオブジェクトの設定")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [SerializeField] private Transform _weaponTrans;
    [Header("移動時に検知するためのRayの設定")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _enemyTypeLayerMask;
    [Tooltip("自身のコライダーとぶつからないように設定する")]
    [SerializeField] private Vector2 EnemyTypeRayOffset = new Vector2(1.25f, 0.5f);

    private CancellationTokenSource _cts;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private GameObject _searchDestination;
    private GameObject _forwardDestination;

    /// <summary>ポーズしたときにVelocityを一旦保存しておくための変数</summary>
    private Vector3 _tempVelocity;
    /// <summary>この座標を基準にしてSearch状態の移動を行うsummary>
    private Vector3 _footPos;
    /// <summary>Pause()が呼ばれるとtrueにResume()が呼ばれるとfalseになる</summary>
    private bool _isPause;

#if UNITY_EDITOR
    /// <summary>EnemyControllerでギズモに表示する用途で使っている</summary>
    public Vector3 FootPos => _footPos;
#endif

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _searchDestination = new GameObject("SearchDestination");
        _forwardDestination = new GameObject("ForwardDestination");
    }

    private void Start()
    {
        FootPosUpdateStart();
    }

    private void OnDisable()
    {
        CancelMoving();
    }

    public void Pause()
    {
        _isPause = true;
        _rigidbody.isKinematic = true;
        _tempVelocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _isPause = false;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _tempVelocity;
    }

    /// <summary>
    /// 一定間隔で足元の座標を更新する
    /// この処理は他のクラスやTimeScaleに影響されないので
    /// Update()内のメソッドだがこのクラス内で実行している
    /// </summary>
    private void FootPosUpdateStart()
    {
        this.UpdateAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(UpdateFootPosInterval))
            .Subscribe(_ => UpdateFootPos())
            .AddTo(this);
    }

    private void UpdateFootPos()
    {
        Vector3 rayOrigin = _transform.position + (Vector3)FootPosRayOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.down, FootPosRayDistance, _groundLayerMask);

        if (hit.collider) _footPos = hit.point;
    }

    /// <summary>
    /// Search状態の移動をする際に呼ばれる
    /// 移動先の座標をSearchDestinationのPositionにセットして
    /// 返すことで移動中に移動先を動かすことが出来る
    /// </summary>
    public Transform GetSearchDestination(float distance, bool useRandomDistance)
    {
        // 左右どちらかに足元から第一引数の距離だけ離れた位置に移動先を設定する
        // ○<----★---->○
        Vector3 dir = Random.value > 0.5f ? Vector3.left : Vector3.right;
        float percentage = useRandomDistance ? Random.value : 1;
        Vector3 targetPos = _footPos + dir * distance * percentage;

        _searchDestination.transform.position = targetPos;

#if UNITY_EDITOR
        Debug.DrawLine(targetPos + Vector3.up, targetPos + Vector3.down, Color.yellow, 1.0f);
#endif

        return _searchDestination.transform;
    }

    /// <summary>
    /// 現在の移動をキャンセルしてその場に留まる
    /// 別の移動先に向かう際はこのメソッドを呼んで現在の移動をキャンセルすること
    /// </summary>
    public void CancelMoving()
    {
        _cts?.Cancel();
        DropVertically();
    }

    /// <summary>
    /// 足元からのRayがヒットしない場合はそのまま落下し
    /// ヒットした場合はPositionをその座標にすることで滑らないようにしている
    /// </summary>
    public void Idle()
    {
        if (_isPause) return;

        RaycastHit2D groundHit = Physics2D.Raycast(_transform.position, Vector3.down, 0.25f, _groundLayerMask);
        if (groundHit)
        {
            _rigidbody.velocity = Vector3.zero;
            _transform.position = groundHit.point;
        }
        else
        {
            DropVertically();
        }

        _rigidbody.isKinematic = groundHit;

#if UNITY_EDITOR
        Color c = groundHit ? Color.blue : Color.red;
        Debug.DrawRay(_transform.position, Vector3.down * 0.5f, c, 0.016f);
#endif
    }

    /// <summary>左右の移動をキャンセルして垂直落下させる</summary>
    private void DropVertically()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>前方に任意の距離だけ移動する</summary>
    public void StartMoveForward(float distance, float moveSpeed)
    {
        // 指定した位置にforwardDestinationを移動させてそこに向かって移動する
        Vector3 pos = transform.position;
        pos.x += _spriteTrans.localScale.x * distance;
        _forwardDestination.transform.position = pos;
        StartMoveToTarget(_forwardDestination.transform, moveSpeed * 2);

#if UNITY_EDITOR
        Debug.DrawRay(pos, Vector3.up * 5, Color.magenta, 3.0f);
#endif
    }

    /// <summary>
    /// このメソッドを外部から呼ぶことで移動を行う
    /// 移動を行う際は必ずCancelMoving()を呼んで現在の移動をキャンセルしてから行うこと
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
        // アイドル状態で無効化しているので移動する際は再度物理演算を有効にする
        _rigidbody.isKinematic = false;

        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
    }

    /// <summary>
    /// FixedUpdate()のタイミングでターゲットに向かって1フレーム分だけ移動する事によって
    /// ターゲットへの移動を行う。引数がTransformのためターゲットが動いていても追従する
    /// </summary>
    private async UniTask MoveToTargetAsync(Transform target, float moveSpeed)
    {
        _cts.Token.ThrowIfCancellationRequested();

        TurnToMoveDirection(target.position);
        while (IsDetectedFloor() && IsUndetectedEnemy())
        {
            if (!_isPause)
            {
                SetVelocityToTarget(target.position, moveSpeed);
                TurnToMoveDirection(target.position);
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
        CancelMoving();
    }

    /// <summary>
    /// Velocityをターゲットの方向に向けることでターゲットへの移動を行う
    /// スローモーションとポーズに対応している
    /// </summary>
    private void SetVelocityToTarget(Vector3 targetPos, float moveSpeed)
    {
        Vector3 velo = targetPos - _transform.position;
        float TimeScale = GameManager.Instance.TimeController.EnemyTime;

        if (velo.sqrMagnitude < moveSpeed / ArrivalTolerance)
        {
            velo = Vector3.zero;
        }
        else
        {
            velo = Vector3.Normalize(velo) * moveSpeed;
        }

        velo.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velo * TimeScale;
    }

    private void TurnToMoveDirection(Vector3 targetPos)
    {
        float diff = targetPos.x - _transform.position.x;
        int dir = (int)Mathf.Sign(diff);
        _spriteTrans.localScale = new Vector3(dir, 1, 1);
        
        Vector3 eyePos = _eyeTrans.localPosition;
        eyePos.x = Mathf.Abs(eyePos.x) * dir;
        _eyeTrans.localPosition = eyePos;

        int angle = dir == 1 ? 0 : 180;
        _eyeTrans.eulerAngles = new Vector3(0, 0, angle);

        Vector3 weaponPos = _weaponTrans.localPosition;
        weaponPos.x = Mathf.Abs(weaponPos.x) * dir;
        _weaponTrans.localPosition = weaponPos;

        _weaponTrans.localScale = new Vector3(dir, 1, 1);
    }

    private bool IsDetectedFloor()
    {
        Vector3 rayOrigin = _transform.position + (Vector3)FloorRayOffset;
        Vector3 dir = ((_transform.right * _spriteTrans.localScale.x) + new Vector3(0, -2f, 0)).normalized;
        RaycastHit2D groundHit = Physics2D.Raycast(rayOrigin, dir, FloorRayDistance, _groundLayerMask);

#if UNITY_EDITOR
        Color c = groundHit ? Color.blue : Color.red;
        Debug.DrawRay(rayOrigin, dir * FloorRayDistance, c, 0.016f);
#endif

        return groundHit;
    }

    private bool IsUndetectedEnemy()
    {
        Vector3 offset = new Vector3(EnemyTypeRayOffset.x * _spriteTrans.localScale.x, EnemyTypeRayOffset.y, 0);
        Vector3 rayOrigin = _transform.position + offset;
        Vector3 dir = _transform.right * _spriteTrans.localScale.x;
        RaycastHit2D enemyHit = Physics2D.Raycast(rayOrigin, dir, EnemyTypeRayDistance, _enemyTypeLayerMask);

#if UNITY_EDITOR
        Color c = !enemyHit ? Color.blue : Color.red;
        Debug.DrawRay(rayOrigin, dir * EnemyTypeRayDistance, c, 0.016f);
#endif

        return !enemyHit;
    }
}
