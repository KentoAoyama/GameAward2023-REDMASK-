using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// �ړ�����ۂɎg�p����N���X
/// 
/// </summary>
[RequireComponent(typeof(WanderingPositionHolder))]
public class MoveBehavior : MonoBehaviour
{
    /// <summary>
    /// �ړ���ɓ��������ۂɂՂ�Ղ邵�Ȃ��悤�ɂ���ׂ̒l
    /// �l��傫������΂�萳�m�Ɉړ���ɂ��ǂ蒅�����A���x����ł͂Ղ�Ղ邵�Ă��܂�
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;
    /// <summary>
    /// ���t���[��Ray���΂��Ȃ��悤�ɁA���낤��o����ړ��͈͂��X�V����Ԋu��ݒ肷��<br></br>
    /// �Ԋu�����߂�΂�萳�m�Ȃ��낤�낪�o����
    /// </summary>
    private static readonly float UpdateWanderingCenterPosInterval = 0.15f;

    [Header("�ړ����Ƀ^�[�Q�b�g�̕����Ɍ��������")]
    [SerializeField] private Transform _spriteTrans;
    [SerializeField] private Transform _eyeTrans;
    [Header("�ړ����x�̐ݒ�")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 4.0f;

    private Rigidbody2D _rigidbody;
    private WanderingPositionHolder _wanderingPositionHolder;
    private CancellationTokenSource _cts;

    // TODO:�f�o�b�O�p�̒l�Ȃ̂ł�����Ƃ����l�ɒ���
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

    /// <summary>���Ԋu�ł��낤��o����ړ��͈͂��X�V����</summary>
    private void InitWanderingCenterPos()
    {
        this.UpdateAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(UpdateWanderingCenterPosInterval))
            .Subscribe(_ => 
            {
                _wanderingPositionHolder.SetWanderingCenterPos();
            })
            .AddTo(this);
    }

    /// <summary>�f�t�H���g����ύX�����l��ݒ肷��</summary>
    private void InitRigidbodySettings()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.angularDrag = 0;
        _rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// Search�X�e�[�g�ɑJ�ڂ����ۂɊO������Ăяo��
    /// ���낤��o����ړ��͈͂̂����A�����_���ȉӏ��Ɉړ�����
    /// </summary>
    public void StartWalkToWanderingTarget()
    {
        Transform target = _wanderingPositionHolder.GetWanderingTarget();
        StartRunToTarget(target);
    }

    /// <summary>�����Ĉړ���Ɍ������ۂɊO������Ăяo��</summary>
    public void StartWalkToTarget(Transform target) => StartMoveToTarget(target, _walkSpeed);

    /// <summary>�����Ĉړ���Ɍ������ۂɊO������Ăяo��</summary>
    public void StartRunToTarget(Transform target) => StartMoveToTarget(target, _runSpeed);

    /// <summary>
    /// ���݂̈ړ����L�����Z�����Ă��̏�ɗ��܂�ۂɊO������Ăяo��
    /// �܂��A�ʂ̈ړ���Ɍ������ۂ͂��̃��\�b�h���Ă�Ō��݂̈ړ����L�����Z�����邱��
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
