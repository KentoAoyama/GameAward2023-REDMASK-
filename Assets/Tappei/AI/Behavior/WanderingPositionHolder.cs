using UnityEngine;

/// <summary>
/// ���낤�낷��ۂɎg�p������W��ێ����Ă���N���X
/// �ړ�����ۂ�MoveController������W�̐ݒ�Ǝ擾���s��
/// </summary>
public class WanderingPositionHolder : MonoBehaviour
{
    /// <summary>
    /// �g�̂̒��S�t�߂���Ray���΂����ƂŃR���C�_�[�̑傫���ɍ��E����ɂ�������
    /// </summary>
    private static readonly float RayOffset = 0.5f;

    [Header("�������m���邽�߂�Ray�̐ݒ�")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _rayDistance = 1.0f;
    [Header("���낤�낷��ۂɈړ����鋗���̐ݒ�")]
    [SerializeField] private float _moveDistance = 3.0f;
    [Tooltip("�ړ������Ƀ����_�������������邩�ǂ���")]
    [SerializeField] private bool _useRandomDistance = true;

    private Vector3 _wanderingCenterPos;
    private GameObject _wanderingTarget;

    private void Awake()
    {
        InitCreateWanderingTarget();
    }

    private void InitCreateWanderingTarget()
    {
        _wanderingTarget = new GameObject("WanderingTarget");
        SetWanderingCenterPos();
    }

    /// <summary>
    /// ������Ray���΂��ē��������ʒu�����낤��̊�ƂȂ���W�Ƃ���
    /// �L�����N�^�[���ړ������ۂ͊�̍��W��ύX����
    /// </summary>
    public void SetWanderingCenterPos()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * RayOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.down, _rayDistance, _groundLayerMask);
        if (hit.collider)
        {
            _wanderingCenterPos = hit.point;
        }
        else
        {
            Debug.LogError("Ray���q�b�g���Ȃ������̂ō��W���Z�b�g�ł��܂���ł���");
        }
    }

    /// <summary>
    /// �ړ���̍��W����̃I�u�W�F�N�g��Position�ɃZ�b�g���Ĉړ���Ƃ��ĕԂ����Ƃ�
    /// ���낤��Ō���������ړ����ɕύX�ł���
    /// </summary>
    public Transform GetWanderingTarget()
    {
        Vector3 dir = Random.value > 0.5f ? Vector3.left : Vector3.right;
        float percentage = _useRandomDistance ? Random.value : 1;
        Vector3 targetPos = _wanderingCenterPos + dir * _moveDistance * percentage;
        _wanderingTarget.transform.position = targetPos;

#if UNITY_EDITOR
        Debug.DrawLine(targetPos + Vector3.up, targetPos + Vector3.down, Color.green, 1.0f);
#endif

        return _wanderingTarget.transform;
    }

    private void OnDrawGizmos()
    {
        if (_wanderingTarget != null)
        {
            // ���낤�낷��͈͂�\������
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_wanderingCenterPos + Vector3.right * _moveDistance, 0.25f);
            Gizmos.DrawSphere(_wanderingCenterPos + Vector3.left * _moveDistance, 0.25f);
        }

        // ���낤��̊�ƂȂ���W�Ɍ�����Ray��\������
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * RayOffset, Vector3.down * _rayDistance);
    }
}
