using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

/// <summary>
/// Šeó‘Ô‚É‚¨‚¢‚ÄˆÚ“®‚·‚éÛ‚Ég—p‚·‚éƒNƒ‰ƒX
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    /// <summary>
    /// ˆÚ“®æ‚É“’…‚µ‚½Û‚É‚Õ‚é‚Õ‚é‚µ‚È‚¢‚æ‚¤‚É‚·‚éˆ×‚Ì’l
    /// ’l‚ğ‘å‚«‚­‚·‚ê‚Î‚æ‚è³Šm‚ÉˆÚ“®æ‚É‚½‚Ç‚è’…‚­‚ªA‘¬“xŸ‘æ‚Å‚Í‚Õ‚é‚Õ‚é‚µ‚Ä‚µ‚Ü‚¤
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;
    /// <summary>
    /// –ˆƒtƒŒ[ƒ€Ray‚ğ”ò‚Î‚³‚È‚¢‚æ‚¤‚ÉASearchó‘Ô‚Å‚ÌˆÚ“®”ÍˆÍ‚ğXV‚·‚éŠÔŠu‚ğİ’è‚·‚é
    /// </summary>
    private static readonly float UpdateFootPosInterval = 0.15f;

    private static readonly float FootPosRayDistance = 1.0f;
    private static readonly float FloorRayDistance = 6.0f;
    private static readonly float EnemyTypeRayDistance = 1.1f;
    private static readonly Vector2 FootPosRayOffset = new Vector2(0, 0.5f);
    private static readonly Vector2 FloorRayOffset = new Vector2(0, 1.5f);

    [Header("ˆÚ“®•ûŒü‚ÉŒü‚¯‚éƒIƒuƒWƒFƒNƒg‚Ìİ’è")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [SerializeField] private Transform _weaponTrans;
    [Header("ˆÚ“®‚ÉŒŸ’m‚·‚é‚½‚ß‚ÌRay‚Ìİ’è")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _enemyTypeLayerMask;
    [Tooltip("©g‚ÌƒRƒ‰ƒCƒ_[‚Æ‚Ô‚Â‚©‚ç‚È‚¢‚æ‚¤‚Éİ’è‚·‚é")]
    [SerializeField] private Vector2 EnemyTypeRayOffset = new Vector2(1.25f, 0.5f);

    private CancellationTokenSource _cts;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private GameObject _searchDestination;
    private GameObject _forwardDestination;

    /// <summary>‚±‚ÌÀ•W‚ğŠî€‚É‚µ‚ÄSearchó‘Ô‚ÌˆÚ“®‚ğs‚¤summary>
    private Vector3 _footPos;

#if UNITY_EDITOR
    /// <summary>EnemyController‚ÅƒMƒYƒ‚‚É•\¦‚·‚é—p“r‚Åg‚Á‚Ä‚¢‚é</summary>
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
        _rigidbody.isKinematic = true;
    }

    /// <summary>
    /// ˆê’èŠÔŠu‚Å‘«Œ³‚ÌÀ•W‚ğXV‚·‚é
    /// ‚±‚Ìˆ—‚Í‘¼‚ÌƒNƒ‰ƒX‚âTimeScale‚É‰e‹¿‚³‚ê‚È‚¢‚Ì‚Å
    /// Update()“à‚Ìƒƒ\ƒbƒh‚¾‚ª‚±‚ÌƒNƒ‰ƒX“à‚ÅÀs‚µ‚Ä‚¢‚é
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
    /// Searchó‘Ô‚ÌˆÚ“®‚ğ‚·‚éÛ‚ÉŒÄ‚Î‚ê‚é
    /// ˆÚ“®æ‚ÌÀ•W‚ğSearchDestination‚ÌPosition‚ÉƒZƒbƒg‚µ‚Ä
    /// •Ô‚·‚±‚Æ‚ÅˆÚ“®’†‚ÉˆÚ“®æ‚ğ“®‚©‚·‚±‚Æ‚ªo—ˆ‚é
    /// </summary>
    public Transform GetSearchDestination(float distance, bool useRandomDistance)
    {
        // ¶‰E‚Ç‚¿‚ç‚©‚É‘«Œ³‚©‚ç‘æˆêˆø”‚Ì‹——£‚¾‚¯—£‚ê‚½ˆÊ’u‚ÉˆÚ“®æ‚ğİ’è‚·‚é
        // ›<----š---->›
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
    /// Œ»İ‚ÌˆÚ“®‚ğƒLƒƒƒ“ƒZƒ‹‚µ‚Ä‚»‚Ìê‚É—¯‚Ü‚é
    /// •Ê‚ÌˆÚ“®æ‚ÉŒü‚©‚¤Û‚Í‚±‚Ìƒƒ\ƒbƒh‚ğŒÄ‚ñ‚ÅŒ»İ‚ÌˆÚ“®‚ğƒLƒƒƒ“ƒZƒ‹‚·‚é‚±‚Æ
    /// </summary>
    public void CancelMoving()
    {
        _cts?.Cancel();
        DropVertically();
    }

    /// <summary>
    /// ‘«Œ³‚©‚ç‚ÌRay‚ªƒqƒbƒg‚µ‚È‚¢ê‡‚Í‚»‚Ì‚Ü‚Ü—‰º‚µ
    /// ƒqƒbƒg‚µ‚½ê‡‚ÍPosition‚ğ‚»‚ÌÀ•W‚É‚·‚é‚±‚Æ‚ÅŠŠ‚ç‚È‚¢‚æ‚¤‚É‚µ‚Ä‚¢‚é
    /// </summary>
    public void Idle()
    {
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

    /// <summary>¶‰E‚ÌˆÚ“®‚ğƒLƒƒƒ“ƒZƒ‹‚µ‚Ä‚’¼—‰º‚³‚¹‚é</summary>
    private void DropVertically()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>ï¿½Oï¿½ï¿½ï¿½É”Cï¿½Ó‚Ì‹ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ú“ï¿½ï¿½ï¿½ï¿½ï¿½</summary>
    public void StartMoveForward(float distance, float moveSpeed)
    {
        // ï¿½wï¿½è‚µï¿½ï¿½ï¿½Ê’uï¿½ï¿½forwardDestinationï¿½ï¿½Ú“ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ä‚ï¿½ï¿½ï¿½ï¿½ÉŒï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ÄˆÚ“ï¿½ï¿½ï¿½ï¿½ï¿½
        Vector3 pos = transform.position;
        pos.x += _spriteTrans.localScale.x * distance;
        _forwardDestination.transform.position = pos;
        StartMoveToTarget(_forwardDestination.transform, moveSpeed * 2);

#if UNITY_EDITOR
        Debug.DrawRay(pos, Vector3.up * 5, Color.magenta, 3.0f);
#endif
    }

    /// <summary>
    /// ‚±‚Ìƒƒ\ƒbƒh‚ğŠO•”‚©‚çŒÄ‚Ô‚±‚Æ‚ÅˆÚ“®‚ğs‚¤
    /// ˆÚ“®‚ğs‚¤Û‚Í•K‚¸CancelMoving()‚ğŒÄ‚ñ‚ÅŒ»İ‚ÌˆÚ“®‚ğƒLƒƒƒ“ƒZƒ‹‚µ‚Ä‚©‚çs‚¤‚±‚Æ
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
        // ƒAƒCƒhƒ‹ó‘Ô‚Å–³Œø‰»‚µ‚Ä‚¢‚é‚Ì‚ÅˆÚ“®‚·‚éÛ‚ÍÄ“x•¨—‰‰Z‚ğ—LŒø‚É‚·‚é
        _rigidbody.isKinematic = false;

        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
    }

    /// <summary>
    /// FixedUpdate()‚Ìƒ^ƒCƒ~ƒ“ƒO‚Åƒ^[ƒQƒbƒg‚ÉŒü‚©‚Á‚Ä1ƒtƒŒ[ƒ€•ª‚¾‚¯ˆÚ“®‚·‚é–‚É‚æ‚Á‚Ä
    /// ƒ^[ƒQƒbƒg‚Ö‚ÌˆÚ“®‚ğs‚¤Bˆø”‚ªTransform‚Ì‚½‚ßƒ^[ƒQƒbƒg‚ª“®‚¢‚Ä‚¢‚Ä‚à’Ç]‚·‚é
    /// </summary>
    private async UniTask MoveToTargetAsync(Transform target, float moveSpeed)
    {
        _cts.Token.ThrowIfCancellationRequested();

        TurnToMoveDirection(target.position);
        while (IsDetectedFloor() && IsUndetectedEnemy())
        {
            SetVelocityToTarget(target.position, moveSpeed);
            TurnToMoveDirection(target.position);

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
        CancelMoving();
    }

    /// <summary>
    /// Velocity‚ğƒ^[ƒQƒbƒg‚Ì•ûŒü‚ÉŒü‚¯‚é‚±‚Æ‚Åƒ^[ƒQƒbƒg‚Ö‚ÌˆÚ“®‚ğs‚¤
    /// ƒXƒ[ƒ‚[ƒVƒ‡ƒ“‚Æƒ|[ƒY‚É‘Î‰‚µ‚Ä‚¢‚é
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
