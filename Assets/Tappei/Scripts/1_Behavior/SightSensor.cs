using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ���̃N���X���g�p���Ď��E���Ƀv���C���[�����邩�ǂ��������o����
/// �X���[���[�V���������ǂ����ɉe������Ȃ�
/// </summary>
public class SightSensor : MonoBehaviour
{
    /// <summary>
    /// ��x�Ɏ��E�����o�ł���I�u�W�F�N�g�̍ő吔
    /// �X�e�[�W�ɑ�ʂ̌��o�ł���I�u�W�F�N�g�����݂���ꍇ�͑��₷�K�v������
    /// </summary>
    private static readonly int MaxDetected = 9;

    [Header("���o�͈͂̊�ƂȂ�I�u�W�F�N�g")]
    [Tooltip("������̏�Q���Ƃ��Č��m���Ă��܂��̂ő��̃R���C�_�[�Ɣ킹�Ȃ�����")]
    [SerializeField] private Transform _eyeTransform;
    [Header("���o����I�u�W�F�N�g�������郌�C���[")]
    [SerializeField] private LayerMask _detectedLayerMask;

#if UNITY_EDITOR
    // �ȉ�2�̃t�B�[���h�̓G�f�B�^��Play���[�h�Ŏ��E�̃M�Y����\�����邽�߂̂���
    private float _radius;
    private float _angle;
#endif

    private Collider2D[] _detectedResults = new Collider2D[MaxDetected];

    /// <returns>
    /// �v���C���[�����E���ɂ���ꍇ�̓v���C���[�Ƃ̋�����Ԃ�
    /// ���E���ɂ��Ȃ��ꍇ�� -1 ���Ԃ�
    /// </returns>
    public float TryGetDistanceToPlayer(float radius, float maxAngle, bool isIgnoreObstacle = false)
    {
#if UNITY_EDITOR
        _radius = radius;
        _angle = maxAngle;
#endif

        Vector3 rayOrigin = _eyeTransform.position;

        // �q�b�g���Ȃ������ꍇ�ł��z����̗v�f�͍폜����Ȃ��̂�
        // �q�b�g�����I�u�W�F�N�g�̏���Ԃ��悤�ɕύX����ꍇ�͒���
        int hitCount = Physics2D.OverlapCircleNonAlloc(rayOrigin, radius, _detectedResults, _detectedLayerMask);
        if (hitCount == 0) return -1;

        foreach (Collider2D detectedCollider in _detectedResults)
        {
            if (detectedCollider == null) break;

            Vector3 targetPos = detectedCollider.transform.position;
            Vector3 targetDir = Vector3.Normalize(targetPos - rayOrigin);
            float angle = Vector3.Angle(targetDir, _eyeTransform.right);

            if (angle > maxAngle / 2) continue;

            float distance = Vector3.Distance(rayOrigin, targetPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, targetDir, distance);

            if (isIgnoreObstacle) return distance;

            // ���E���Ղ�I�u�W�F�N�g�p�̃��C���[������΁A�^�[�Q�b�g�܂ł�Ray���΂���
            // ���E���Ղ�I�u�W�F�N�g�Ƀq�b�g�����王�E�ɉf��Ȃ��Ƃ��������ɕύX�o����B
            bool isSightable = hit.collider.GetInstanceID() == detectedCollider.GetInstanceID();

#if UNITY_EDITOR
            Color color = isSightable ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, targetDir * distance, color);
#endif
            if (isSightable) return distance;
        }

        return -1;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (_eyeTransform != null)
        {
            DrawSearchArea();
        }
    }

    private void DrawSearchArea()
    {
        Handles.color = new Color32(0, 0, 255, 64);
        Vector3 dir = Quaternion.Euler(0, 0, -_angle / 2) * _eyeTransform.right;
        Handles.DrawSolidArc(_eyeTransform.position, Vector3.forward, dir, _angle, _radius);
    }
#endif
}
