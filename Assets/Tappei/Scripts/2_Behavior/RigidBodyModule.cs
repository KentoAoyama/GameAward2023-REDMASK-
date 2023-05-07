using UnityEngine;

/// <summary>
/// Rigidbody�𑀍삷��N���X
/// MoveBehavior�N���X����g�p�����
/// </summary>
[System.Serializable]
public class RigidBodyModule
{
    /// <summary>
    /// �ړ���ɓ��������ۂɂՂ�Ղ邵�Ȃ��悤�ɂ���ׂ̒l
    /// �l��傫������΂�萳�m�Ɉړ���ɂ��ǂ蒅�����A���x����ł͂Ղ�Ղ邵�Ă��܂�
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;

    [SerializeField] private Rigidbody2D _rigidbody;
    
    /// <summary>
    /// �|�[�Y�����Ƃ���Velocity����U�ۑ����Ă������߂̕ϐ�
    /// </summary>
    private Vector3 _tempVelocity;

    /// <summary>
    /// Velocity���^�[�Q�b�g�̕����Ɍ����邱�ƂŃ^�[�Q�b�g�ւ̈ړ����s��
    /// �X���[���[�V�����ƃ|�[�Y�ɑΉ����Ă���
    /// </summary>
    public void SetVelocityToTarget(Vector3 targetPos, float moveSpeed, Transform transform)
    {
        Vector3 velo = targetPos - transform.position;

        bool isArrival = velo.sqrMagnitude < moveSpeed / ArrivalTolerance;
        velo = isArrival ? Vector3.zero : Vector3.Normalize(velo) * moveSpeed;
        velo.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velo * GameManager.Instance.TimeController.EnemyTime;
    }

    /// <summary>
    /// ���R����������ׂ�Velocity��ݒ肷��
    /// �ړ����L�����Z�������ꍇ�ɌĂ΂��
    /// </summary>
    public void SetFallVelocity()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>
    /// �⓹�ŐÎ~���Ă���ۂ̊���~�߂��s������
    /// �ړ��J�n���ɂ�isKinematic�̗L�����̂��߂ɌĂ΂��
    /// </summary>
    public void UpdateKinematic(bool isKinematic)
    {
        if (isKinematic)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            SetFallVelocity();
        }

        _rigidbody.isKinematic = isKinematic;
    }

    /// <summary>
    /// �|�[�Y���ɑ��x���ꎞ�I�ɕۑ�����
    /// </summary>
    public void SaveVelocity()
    {
        _rigidbody.isKinematic = true;
        _tempVelocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// �|�[�Y�������Ɉꎞ�ۑ����Ă������x�����蓖�Ă�
    /// </summary>
    public void LoadVelocity()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _tempVelocity;
    }
}
