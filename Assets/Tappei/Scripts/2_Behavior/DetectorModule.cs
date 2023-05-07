using UnityEngine;

/// <summary>
/// Ray���΂��ăI�u�W�F�N�g�����m����N���X
/// MoveBehavior�N���X����g�p�����
/// </summary>
[System.Serializable]
public class DetectorModule
{
    [System.Serializable]
    public struct RaySettings
    {
        public float _distance;
        public Vector3 _offset;
        public LayerMask _layerMask;
    }

    [Tooltip("����Ray�͍��E�̌����ɉ����Ĕ��]���Ȃ��̂Œ���")]
    [SerializeField] private RaySettings _footPosRaySettings;
    [Tooltip("����Ray�͍��E�̌����ɉ����Ĕ��]���Ȃ��̂Œ���")]
    [SerializeField] private RaySettings _floorRaySettings;
    [Tooltip("���g�̃R���C�_�[�ƂԂ���Ȃ��悤�ɐݒ肷��K�v������")]
    [SerializeField] private RaySettings _enemyRaySettings;

    /// <summary>
    /// ����(�^��)�Ɍ�������Ray���΂����Ƃŏ��̏�ɂ��邩�𔻒肷��
    /// ���̏�ɂ���ꍇ�͂��̍��W��Ԃ����ƂŃL�����N�^�[�̑����̊�ɂȂ���W���X�V����
    /// </summary>
    public bool DetectFootPos(Transform transform, out Vector3 hitPos)
    {
        Vector3 rayOrigin = transform.position + _footPosRaySettings._offset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.down, 
            _footPosRaySettings._distance, _footPosRaySettings._layerMask);

        hitPos = hit.collider ? hit.point : Vector3.zero;
        return hit.collider;
    }

    /// <summary>
    /// �΂ߑO��Ray���΂����ƂŐi�񂾐�ɏ������邩�𔻒肷��
    /// </summary>
    public bool DetectFloorInFront(int dir, Transform transform)
    {
        // Ray�̎΂߉��̌�������
        float y = -2.0f;

        Vector3 rayOrigin = transform.position + _floorRaySettings._offset;
        Vector3 rayDir = ((transform.right * dir) + new Vector3(0, y, 0)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, 
            _floorRaySettings._distance, _floorRaySettings._layerMask);

#if UNITY_EDITOR
        DebugDrawRay(hit, rayOrigin, rayDir * _floorRaySettings._distance);
#endif

        return hit;
    }

    /// <summary>
    /// ���ʂɌ�������Ray���΂����ƂőO���ɓG�����邩�ǂ����𔻒肷��
    /// ���ݎg���Ă��Ȃ����ǉ��̋@�\�����ۂɕK�v�ɂȂ肻���Ȃ̂Ŏc���Ă���
    /// </summary>
    public bool DetectEnemyInForward(int dir, Transform transform)
    {
        Vector3 offset = new Vector3(_enemyRaySettings._offset.x * dir, _enemyRaySettings._offset.y, 0);
        Vector3 rayOrigin = transform.position + offset;
        Vector3 rayDir = transform.right * dir;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, 
            _enemyRaySettings._distance, _enemyRaySettings._layerMask);

#if UNITY_EDITOR
        DebugDrawRay(!hit, rayOrigin, rayDir * _enemyRaySettings._distance);
#endif

        return !hit;
    }

    private void DebugDrawRay(bool hit, Vector3 rayOrigin, Vector3 dir)
    {
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, dir * _enemyRaySettings._distance, color, 0.016f);
    }
}
