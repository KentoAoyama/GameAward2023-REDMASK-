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
    /// <summary>
    /// �g�̂̒��S�t�߂���Ray���΂����ƂŃR���C�_�[�̑傫���ɍ��E����ɂ�������
    /// </summary>
    private static readonly float RayOffset = 0.5f;

    [Header("�ړ������Ɍ�����I�u�W�F�N�g�̐ݒ�")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [SerializeField] private Transform _weaponTrans;
    [Header("�������m���邽�߂�Ray�̐ݒ�")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _rayDistance = 1.0f;

    private CancellationTokenSource _cts;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private GameObject _searchDestination;

    /// <summary>���̍��W����ɂ���Search��Ԃ̈ړ����s��summary>
    private Vector3 _footPos;

#if UNITY_EDITOR
    /// <summary>EnemyController�ŃM�Y���ɕ\������p�r�Ŏg���Ă���</summary>
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
        Debug.DrawLine(targetPos + Vector3.up, targetPos + Vector3.down, Color.green, 1.0f);
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

        // ���E�̈ړ����~�߂邪���������͏d�͂ɏ]��
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>
    /// ���̃��\�b�h���O������ĂԂ��Ƃňړ����s��
    /// �ړ����s���ۂ͕K��CancelMoving()���Ă�Ō��݂̈ړ����L�����Z�����Ă���s������
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
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

        while (true)
        {
            SetVelocityToTarget(target.position, moveSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cts.Token);
        }
    }

    /// <summary>
    /// Velocity���^�[�Q�b�g�̕����Ɍ����邱�ƂŃ^�[�Q�b�g�ւ̈ړ����s��
    /// �X���[���[�V�����ƃ|�[�Y�ɑΉ����Ă���
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
