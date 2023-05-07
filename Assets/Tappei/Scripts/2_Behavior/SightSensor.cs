using UnityEngine;

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
    [SerializeField] private LayerMask _playerLayerMask;
    [Tooltip("���E���Ղ�I�u�W�F�N�g�̃��C���[�����蓖�Ă�")]
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Collider2D[] _detectedResults = new Collider2D[MaxDetected];

    /// <summary>
    /// �v���C���[�Ƃ̈ʒu�֌W��񋓌^�ŕԂ�
    /// ���̃��\�b�h���O������ĂԂ��ƂŃv���C���[�Ƃ̈ʒu�ɉ������������s��
    /// </summary>
    public SightResult LookForPlayerInSight(float radius, float maxAngle, float attackRange,
        bool isIgnoreObstacle = false)
    {
        if (TryGetDistanceToPlayer(radius, maxAngle, out float result, isIgnoreObstacle))
        {
            if (result <= attackRange)
            {
                return SightResult.InAttackRange;
            }
            else
            {
                return SightResult.InSight;
            }
        }
        else
        {
            return SightResult.OutSight;
        }
    }

    /// <returns>
    /// �v���C���[�����E���ɂ���ꍇ�̓v���C���[�Ƃ̋�����Ԃ�
    /// ���E���ɂ��Ȃ��ꍇ��-1���Ԃ�
    /// </returns>
    private bool TryGetDistanceToPlayer(float radius, float maxAngle, out float result, 
        bool isIgnoreObstacle = false)
    {
        Vector3 rayOrigin = _eyeTransform.position;

        // �q�b�g���Ȃ������ꍇ�ł��z����̗v�f�͍폜����Ȃ��̂�
        // �q�b�g�����I�u�W�F�N�g�̏���Ԃ��悤�ɕύX����ꍇ�͒���
        int hitCount = Physics2D.OverlapCircleNonAlloc(rayOrigin, radius, _detectedResults, _playerLayerMask);
        if (hitCount == 0)
        {
            result = -1;
            return false;
        }

        foreach (Collider2D detectedCollider in _detectedResults)
        {
            if (detectedCollider == null) break;

            Vector3 targetPos = detectedCollider.transform.position;
            Vector3 targetDir = Vector3.Normalize(targetPos - rayOrigin);
            float angle = Vector3.Angle(targetDir, _eyeTransform.right);

            if (angle > maxAngle / 2) continue;

            float distance = Vector3.Distance(rayOrigin, targetPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, targetDir, distance, _obstacleLayerMask);

            if (isIgnoreObstacle)
            {
                result = distance;
                return true;
            }

            // ���E���Ղ�I�u�W�F�N�g�p�̃��C���[������΁A�^�[�Q�b�g�܂ł�Ray���΂���
            // ���E���Ղ�I�u�W�F�N�g�Ƀq�b�g�����王�E�ɉf��Ȃ��Ƃ��������ɕύX�o����B
            //bool isSightable = hit.collider.GetInstanceID() == detectedCollider.GetInstanceID();
            bool isSightable = !hit;

#if UNITY_EDITOR
            Color color = isSightable ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, targetDir * distance, color);
#endif
            if (isSightable)
            {
                result = distance;
                return true;
            }
        }

        result = -1;
        return false;
    }
}
