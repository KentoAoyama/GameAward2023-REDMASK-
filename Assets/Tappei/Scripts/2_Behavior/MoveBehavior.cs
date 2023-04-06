using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

/// <summary>
/// �e��Ԃɂ����Ĉړ�����ۂɎg�p����N���X
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    /// <summary>
    /// �ړ���ɓ��������ۂɂՂ�Ղ邵�Ȃ��悤�ɂ���ׂ̒l
    /// �l��傫������΂�萳�m�Ɉړ���ɂ��ǂ蒅�����A���x����ł͂Ղ�Ղ邵�Ă��܂�
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;
    /// <summary>
    /// ���t���[��Ray���΂��Ȃ��悤�ɁASearch��Ԃł̈ړ��͈͂��X�V����Ԋu��ݒ肷��
    /// </summary>
    private static readonly float UpdateFootPosInterval = 0.15f;

    private static readonly float FootPosRayDistance = 1.0f;
    private static readonly float FloorRayDistance = 6.0f;
    private static readonly float EnemyTypeRayDistance = 1.1f;
    private static readonly Vector2 FootPosRayOffset = new Vector2(0, 0.5f);
    private static readonly Vector2 FloorRayOffset = new Vector2(0, 1.5f);

    [Header("�ړ������Ɍ�����I�u�W�F�N�g�̐ݒ�")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [SerializeField] private Transform _weaponTrans;
    [Header("�ړ����Ɍ��m���邽�߂�Ray�̐ݒ�")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _enemyTypeLayerMask;
    [Tooltip("���g�̃R���C�_�[�ƂԂ���Ȃ��悤�ɐݒ肷��")]
    [SerializeField] private Vector2 EnemyTypeRayOffset = new Vector2(1.25f, 0.5f);

    private CancellationTokenSource _cts;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private GameObject _searchDestination;
    private GameObject _forwardDestination;

    /// <summary>�|�[�Y�����Ƃ���Velocity����U�ۑ����Ă������߂̕ϐ�</summary>
    private Vector3 _tempVelocity;
    /// <summary>���̍��W����ɂ���Search��Ԃ̈ړ����s��summary>
    private Vector3 _footPos;
    /// <summary>Pause()���Ă΂���true��Resume()���Ă΂���false�ɂȂ�</summary>
    private bool _isPause;

#if UNITY_EDITOR
    /// <summary>EnemyController�ŃM�Y���ɕ\������p�r�Ŏg���Ă���</summary>
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
    /// ���Ԋu�ő����̍��W���X�V����
    /// ���̏����͑��̃N���X��TimeScale�ɉe������Ȃ��̂�
    /// Update()���̃��\�b�h�������̃N���X���Ŏ��s���Ă���
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
    /// Search��Ԃ̈ړ�������ۂɌĂ΂��
    /// �ړ���̍��W��SearchDestination��Position�ɃZ�b�g����
    /// �Ԃ����Ƃňړ����Ɉړ���𓮂������Ƃ��o����
    /// </summary>
    public Transform GetSearchDestination(float distance, bool useRandomDistance)
    {
        // ���E�ǂ��炩�ɑ�������������̋����������ꂽ�ʒu�Ɉړ����ݒ肷��
        // ��<----��---->��
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
    /// ���݂̈ړ����L�����Z�����Ă��̏�ɗ��܂�
    /// �ʂ̈ړ���Ɍ������ۂ͂��̃��\�b�h���Ă�Ō��݂̈ړ����L�����Z�����邱��
    /// </summary>
    public void CancelMoving()
    {
        _cts?.Cancel();
        DropVertically();
    }

    /// <summary>
    /// ���������Ray���q�b�g���Ȃ��ꍇ�͂��̂܂ܗ�����
    /// �q�b�g�����ꍇ��Position�����̍��W�ɂ��邱�ƂŊ���Ȃ��悤�ɂ��Ă���
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

    /// <summary>���E�̈ړ����L�����Z�����Đ�������������</summary>
    private void DropVertically()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>�O���ɔC�ӂ̋��������ړ�����</summary>
    public void StartMoveForward(float distance, float moveSpeed)
    {
        // �w�肵���ʒu��forwardDestination���ړ������Ă����Ɍ������Ĉړ�����
        Vector3 pos = transform.position;
        pos.x += _spriteTrans.localScale.x * distance;
        _forwardDestination.transform.position = pos;
        StartMoveToTarget(_forwardDestination.transform, moveSpeed * 2);

#if UNITY_EDITOR
        Debug.DrawRay(pos, Vector3.up * 5, Color.magenta, 3.0f);
#endif
    }

    /// <summary>
    /// ���̃��\�b�h���O������ĂԂ��Ƃňړ����s��
    /// �ړ����s���ۂ͕K��CancelMoving()���Ă�Ō��݂̈ړ����L�����Z�����Ă���s������
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
        // �A�C�h����ԂŖ��������Ă���̂ňړ�����ۂ͍ēx�������Z��L���ɂ���
        _rigidbody.isKinematic = false;

        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
    }

    /// <summary>
    /// FixedUpdate()�̃^�C�~���O�Ń^�[�Q�b�g�Ɍ�������1�t���[���������ړ����鎖�ɂ����
    /// �^�[�Q�b�g�ւ̈ړ����s���B������Transform�̂��߃^�[�Q�b�g�������Ă��Ă��Ǐ]����
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
    /// Velocity���^�[�Q�b�g�̕����Ɍ����邱�ƂŃ^�[�Q�b�g�ւ̈ړ����s��
    /// �X���[���[�V�����ƃ|�[�Y�ɑΉ����Ă���
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
