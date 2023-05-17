using UnityEngine;

/// <summary>
/// �G�̍U�����A����̗\������\������N���X
/// </summary>
public class EnemyWeaponGuidelineDrawer : MonoBehaviour
{
    [Tooltip("�\�������Ւf���郌�C���[�A�v���C���[�ƕ�&�n�ʂ�z��")]
    [SerializeField] private LayerMask _obstacleLayerMask;
    [Header("�\�����̐ݒ�")]
    [SerializeField] private float _maxDistance = 10;
    [SerializeField] private float _width = 0.05f;

    private LineRenderer _lineRenderer;
    /// <summary>
    /// �ǂꂩ1�ɂł��������Ă���΂ŏ����𕪊򂷂�̂ŗe�ʂ�1�ŏ\��
    /// </summary>
    private RaycastHit2D[] _guidelineRayResult = new RaycastHit2D[1];
    private bool _isDraw;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = _width;
    }

    private void Update()
    {
        _isDraw = false;
    }

    private void LateUpdate()
    {
        if (!_isDraw)
        {
            _lineRenderer.enabled = false;
        }
    }

    public void Draw(Vector3 rayOrigin, Vector3 dir)
    {
        _isDraw = true;
        _lineRenderer.enabled = true;
        Physics2D.RaycastNonAlloc(rayOrigin, dir, _guidelineRayResult, _maxDistance, _obstacleLayerMask);
        
        Vector3 to;
        if (_guidelineRayResult[0])
        {
            to = _guidelineRayResult[0].point;
        }
        else
        {
            to = transform.position + dir * _maxDistance;
        }

        _lineRenderer.SetPositions(new Vector3[] 
        {
            transform.position,
            to
        });
    }
}
