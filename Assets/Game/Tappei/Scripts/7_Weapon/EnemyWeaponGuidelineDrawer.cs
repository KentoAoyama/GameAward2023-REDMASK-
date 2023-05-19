using UnityEngine;

/// <summary>
/// 敵の攻撃時、武器の予告線を表示するクラス
/// </summary>
public class EnemyWeaponGuidelineDrawer : MonoBehaviour
{
    [Tooltip("予告線を遮断するレイヤー、プレイヤーと壁&地面を想定")]
    [SerializeField] private LayerMask _obstacleLayerMask;
    [Header("予告線の設定")]
    [SerializeField] private float _maxDistance = 10;
    [SerializeField] private float _width = 0.05f;

    private LineRenderer _lineRenderer;
    /// <summary>
    /// どれか1つにでも当たっていればで処理を分岐するので容量は1で十分
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
