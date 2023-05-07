using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// �e��Ԃɂ����Ĉړ�����ۂɎg�p����N���X
/// �K�v�ȊeModule�̃N���X�𐧌䂷��
/// </summary>
public class MoveBehavior : MonoBehaviour
{
    private Transform _transform;
    private CancellationTokenSource _cts;

    [SerializeField] private TurnModule _turnModule;
    [SerializeField] private DetectorModule _detectorModule;
    [SerializeField] private RigidBodyModule _rigidbodyModule;
    [SerializeField] private WaypointModule _waypointModule;
    [Tooltip("Sprite�̍��E�ɉ������������������̂ŎQ�Ƃ��K�v")]
    [SerializeField] private Transform _sprite;

    /// <summary>
    /// Pause()���Ă΂���true��Resume()���Ă΂���false�ɂȂ�
    /// </summary>
    private bool _isPause;

    /// <summary>
    /// Sprite�̍��E�̌����ɍ��킹������������ۂɎg��
    /// �E����: 1 ������: -1
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
    /// ���Ԋu�ő����̍��W���X�V����
    /// ���̏����͑��̃N���X��TimeScale�ɉe������Ȃ��̂�
    /// Update()���̃��\�b�h�������̃N���X���Ŏ��s���Ă���
    /// </summary>
    private void FootPosUpdateStart()
    {
        // ���t���[��Ray���΂��Ȃ��悤�ɁASearch��Ԃł̈ړ��͈͂��X�V����Ԋu��ݒ肷��
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
    /// �L�����N�^�[���������ɔz�u����ۂɎg�p����
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
    /// �^�[�Q�b�g�Ɍ����Ĉړ�����
    /// ��{�͂��̃��\�b�h���ĂԂ��ƂŃ^�[�Q�b�g��ǂ�������
    /// </summary>
    public void StartMoveToTarget(Transform target, float moveSpeed)
    {
        _cts = new CancellationTokenSource();
        MoveToTargetAsync(target, moveSpeed).Forget();
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
    /// ���݂̈ړ����L�����Z�����Ă��̏�ɗ��܂�
    /// �ʂ̈ړ���Ɍ������ۂ͂��̃��\�b�h���Ă�Ō��݂̈ړ����L�����Z�����邱��
    /// </summary>
    public void CancelMoveToTarget()
    {
        _cts?.Cancel();
        _rigidbodyModule.SetFallVelocity();
    }

    /// <summary>
    /// ���������Ray���q�b�g���Ȃ��ꍇ�͂��̂܂ܗ�����
    /// �q�b�g�����ꍇ��Position�����̍��W�ɂ��邱�ƂŊ���Ȃ��悤�ɂ��Ă���
    /// </summary>
    public void Idle()
    {
        if (_isPause) return;

        bool onGround = _detectorModule.DetectOnGroundIdle(_transform.position, out Vector3 hitPos);
        _rigidbodyModule.UpdateKinematic(onGround);
    }
}
