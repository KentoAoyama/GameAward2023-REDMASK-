using UnityEngine;

/// <summary>
/// ���̃N���X���g�p���Ď��E���Ƀv���C���[�����邩�ǂ��������o����
/// �X���[���[�V���������ǂ����ɉe������Ȃ�
/// </summary>
public class SightSensor : MonoBehaviour
{
    /// <summary>�v���C���[�����E���ɂ��Ȃ��ꍇ�ɕԂ�l</summary>
    public static readonly int PlayerOutSight = -1;

    /// <summary>
    /// ��x�Ɏ��E�����o�ł���I�u�W�F�N�g�̍ő吔
    /// �X�e�[�W�ɑ�ʂ̌��o�ł���I�u�W�F�N�g�����݂���ꍇ�͑��₷�K�v������
    /// </summary>
    private static readonly int MaxDetected = 9;

    [Header("���o�͈͂̊�ƂȂ�I�u�W�F�N�g")]
    [Tooltip("������̏�Q���Ƃ��Č��m���Ă��܂��̂ő��̃R���C�_�[�Ɣ킹�Ȃ�����")]
    [SerializeField] private Transform _eyeTransform;
    [Header("���o����I�u�W�F�N�g�������郌�C���[")]
    [SerializeField] private LayerMask _playerLayerMask;
    [Tooltip("���E���Ղ�I�u�W�F�N�g�̃��C���[")]
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Collider2D[] _detectedResults = new Collider2D[MaxDetected];

#if UNITY_EDITOR
    /// <summary>EnemyController�ŃM�Y���ɕ\������p�r�Ŏg���Ă���</summary>
    public Transform EyeTransform => _eyeTransform;
#endif

    /// <returns>
    /// �v���C���[�����E���ɂ���ꍇ�̓v���C���[�Ƃ̋�����Ԃ�
    /// ���E���ɂ��Ȃ��ꍇ�� PlayerOutSight ���Ԃ�
    /// </returns>
    public float TryGetDistanceToPlayer(float radius, float maxAngle, bool isIgnoreObstacle = false)
    {
        Vector3 rayOrigin = _eyeTransform.position;

        // �q�b�g���Ȃ������ꍇ�ł��z����̗v�f�͍폜����Ȃ��̂�
        // �q�b�g�����I�u�W�F�N�g�̏���Ԃ��悤�ɕύX����ꍇ�͒���
        int hitCount = Physics2D.OverlapCircleNonAlloc(rayOrigin, radius, _detectedResults, _playerLayerMask);
        if (hitCount == 0) return PlayerOutSight;

        foreach (Collider2D detectedCollider in _detectedResults)
        {
            if (detectedCollider == null) break;

            Vector3 targetPos = detectedCollider.transform.position;
            Vector3 targetDir = Vector3.Normalize(targetPos - rayOrigin);
            float angle = Vector3.Angle(targetDir, _eyeTransform.right);

            if (angle > maxAngle / 2) continue;

            float distance = Vector3.Distance(rayOrigin, targetPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, targetDir, distance, _obstacleLayerMask);

            if (isIgnoreObstacle) return distance;

            // ���E���Ղ�I�u�W�F�N�g�p�̃��C���[������΁A�^�[�Q�b�g�܂ł�Ray���΂���
            // ���E���Ղ�I�u�W�F�N�g�Ƀq�b�g�����王�E�ɉf��Ȃ��Ƃ��������ɕύX�o����B
            //bool isSightable = hit.collider.GetInstanceID() == detectedCollider.GetInstanceID();
            bool isSightable = !hit;

#if UNITY_EDITOR
            Color color = isSightable ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, targetDir * distance, color);
#endif
            if (isSightable) return distance;
        }

        return PlayerOutSight;
    }
}
