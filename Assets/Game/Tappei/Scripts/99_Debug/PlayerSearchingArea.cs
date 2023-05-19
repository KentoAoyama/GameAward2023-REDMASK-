using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ���̃N���X���g�p���Ĕ͈͓��Ƀv���C���[�����邩�ǂ��������o����B
/// </summary>
public class PlayerSearchingArea : MonoBehaviour
{
    static readonly int MaxDetected = 9;

    [Header("���o�͈͂̊�ƂȂ�Transform")]
    [Tooltip("������̏�Q���Ƃ��Č��m���Ă��܂��̂ő��̃R���C�_�[�Ɣ킹�Ȃ�����")]
    [SerializeField] Transform _eyeTransform;
    [Header("���o�͈͂̐ݒ�")]
    [SerializeField] float _radius;
    [SerializeField] float _angle;
    [Header("���o����I�u�W�F�N�g�������郌�C���[")]
    [SerializeField] LayerMask _detectedLayerMask;
    [Header("�Ԃɏ�Q�����������ꍇ�ɖ���������")]
    [SerializeField] bool _isIgnoreObstacle;

    Collider2D[] _detectedResults = new Collider2D[MaxDetected];

    void Update()
    {
        Debug.Log(IsDetected());
    }

    /// <summary>�v���C���[�����o�͈͓��ɂ��邩�ǂ�����Ԃ�</summary>
    public bool IsDetected()
    {
        Vector3 rayOrigin = _eyeTransform.position;

        // �q�b�g���Ȃ������ꍇ�ł��z����̗v�f�͍폜����Ȃ��̂�
        // �q�b�g�����I�u�W�F�N�g�̏���Ԃ��悤�ɕύX����ꍇ�͒���
        int hitCount = Physics2D.OverlapCircleNonAlloc(rayOrigin, _radius, _detectedResults, _detectedLayerMask);
        if (hitCount == 0) return false;

        foreach (Collider2D detectedCollider in _detectedResults)
        {
            if (detectedCollider == null) break;
            
            Vector3 targetPos = detectedCollider.transform.position;
            Vector3 targetDir = Vector3.Normalize(targetPos - rayOrigin);
            float angle = Vector3.Angle(targetDir, _eyeTransform.right);

            if (angle > _angle / 2) continue;

            float distance = Vector3.Distance(rayOrigin, targetPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, targetDir, distance);

            if (_isIgnoreObstacle) return true;

            // ���E���Ղ�I�u�W�F�N�g�p�̃��C���[������΁A�^�[�Q�b�g�܂ł�Ray���΂���
            // ���E���Ղ�I�u�W�F�N�g�Ƀq�b�g�����王�E�ɉf��Ȃ��Ƃ��������ɕύX�o����B
            bool isSightable = hit.collider.GetInstanceID() == detectedCollider.GetInstanceID();

#if UNITY_EDITOR
            Color color = isSightable ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, targetDir * distance, color);
#endif
            if (isSightable) return true;
        }

        return false;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (_eyeTransform != null)
        {
            DrawSearchArea();
        }
    }

    void DrawSearchArea()
    {
        Handles.color = new Color32(0, 0, 255, 64);
        Vector3 dir = Quaternion.Euler(0, 0, -_angle / 2) * _eyeTransform.right;
        Handles.DrawSolidArc(_eyeTransform.position, Vector3.forward, dir, _angle, _radius);
    }
#endif
}
