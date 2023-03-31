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
    /// <summary>
    /// 身体の中心付近からRayを飛ばすことでコライダーの大きさに左右されにくくする
    /// </summary>
    private static readonly float RayOffset = 0.5f;

    [Header("移動方向に向けるオブジェクトの設定")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [SerializeField] private Transform _weaponTrans;
    [Header("床を検知するためのRayの設定")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _rayDistance = 1.0f;

    private CancellationTokenSource _cts;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private GameObject _searchDestination;

    /// <summary>この座標を基準にしてSearch状態の移動を行うsummary>
    private Vector3 _footPos;

#if UNITY_EDITOR
    /// <summary>EnemyControllerでギズモに表示する用途で使っている</summary>
    public Vector3 FootPos => _footPos;
#endif

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _searchDestination = new GameObject("SearchDestination");
    }

    private void Start()
    {
        FootPosUpdateStart();
    }

    private void OnDisable()
    {
        CancelMoving();
        _rigidbody.isKinematic = true;
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
        Vector3 rayOrigin = _transform.position + Vector3.up * RayOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.down, _rayDistance, _groundLayerMask);

        if (hit.collider) _footPos = hit.point;

#if UNITY_EDITOR
        Color color = hit.collider ? Color.green : Color.red;
        Vector3 pos = _transform.position;
        Debug.DrawLine(pos + Vector3.up, pos + Vector3.down, color, 1.0f);
#endif
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
        Debug.DrawLine(targetPos + Vector3.up, targetPos + Vector3.down, Color.green, 1.0f);
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

        // 左右の移動を止めるが落下だけは重力に従う
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>
    /// このメソッドを外部から呼ぶことで移動を行う
    /// 移動を行う際は必ずCancelMoving()を呼んで現在の移動をキャンセルしてから行うこと
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
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

        while (true)
        {
            SetVelocityToTarget(target.position, moveSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
    }

    /// <summary>
    /// Velocityをターゲットの方向に向けることでターゲットへの移動を行う
    /// スローモーションとポーズに対応している
    /// </summary>
    private void SetVelocityToTarget(Vector3 targetPos, float moveSpeed)
    {
        Vector3 velo = targetPos - transform.position;
        float TimeScale = GameManager.Instance.TimeController.EnemyTime;

        if (velo.sqrMagnitude < moveSpeed / ArrivalTolerance)
        {
            velo = Vector3.zero;
        }
        else
        {
            velo = Vector3.Normalize(velo) * moveSpeed;
        }

        TurnToMoveDirection(targetPos);

        velo.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velo * TimeScale;
    }

    private void TurnToMoveDirection(Vector3 targetPos)
    {
        float diff = targetPos.x - transform.position.x;
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
}
