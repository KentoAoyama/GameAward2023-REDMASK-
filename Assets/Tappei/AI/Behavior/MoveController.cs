using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �ړ�����̏����𐧌䂷��N���X
/// </summary>
[RequireComponent(typeof(WanderingPositionHolder))]
public class MoveController : MonoBehaviour
{
    /// <summary>
    /// �ړ���ɓ��������ۂɂՂ�Ղ邵�Ȃ��悤�ɂ���ׂ̒l
    /// �l��傫������΂�萳�m�Ɉړ���ɂ��ǂ蒅�����A���x����ł͂Ղ�Ղ邵�Ă��܂�
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;

    [SerializeField] private Rigidbody2D _rigidbody;
    [Header("�ړ����x�̐ݒ�")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 4.0f;

    private WanderingPositionHolder _wanderingPositionHolder;
    private CancellationTokenSource _cts;

    // TODO:�f�o�b�O�p�̒l�Ȃ̂ł�����Ƃ����l�ɒ���
    private float _debugTimeSpeed = 1;

    private void Awake()
    {
        _wanderingPositionHolder = GetComponent<WanderingPositionHolder>();
        InitRigidbodySettings();
    }

    // �e�X�g�p�̌Ăяo��
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
    /// �f�t�H���g����ύX�����l��ݒ肷��
    /// �C���X�y�N�^�[�ł̕ύX�Y��h�~
    /// </summary>
    private void InitRigidbodySettings()
    {
        _rigidbody.angularDrag = 0;
        _rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// �����Ĉړ���Ɍ������ۂɊO������Ăяo��
    /// �ʂ̈ړ���Ɍ������ۂ�CancelMoving()���Ă�Ō��݂̈ړ����L�����Z�����邱��
    /// </summary>
    public void StartWalkToTarget(Transform target) => StartMoveToTarget(target, _walkSpeed);
    /// <summary>
    /// �����Ĉړ���Ɍ������ۂɊO������Ăяo��
    /// �ʂ̈ړ���Ɍ������ۂ�CancelMoving()���Ă�Ō��݂̈ړ����L�����Z�����邱��
    /// </summary>
    public void StartRunToTarget(Transform target) => StartMoveToTarget(target, _runSpeed);

    /// <summary>
    /// ���݂̈ړ����L�����Z�����Ă��̏�ɗ��܂�ۂɊO������Ăяo��
    /// �ʂ̈ړ���Ɍ������ۂ͂��̃��\�b�h���Ă�Ō��݂̈ړ����L�����Z�����邱��
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
    /// FixedUpdate()�̃^�C�~���O�Ń^�[�Q�b�g�Ɍ�������1�t���[���������ړ����鎖�ɂ����
    /// �^�[�Q�b�g�ւ̈ړ����s��<br></br>
    /// ������Transform�̂��߃^�[�Q�b�g�������Ă��Ă��Ǐ]����
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
    /// Velocity���^�[�Q�b�g�̕����Ɍ����邱�ƂŃ^�[�Q�b�g�ւ̈ړ����s��
    /// �X���[���[�V�����ɑΉ����Ă���
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
