using UnityEngine;

/// <summary>
/// うろうろする際に使用する座標を保持しているクラス
/// 移動する際にMoveControllerから座標の設定と取得を行う
/// </summary>
public class WanderingPositionHolder : MonoBehaviour
{
    /// <summary>
    /// 身体の中心付近からRayを飛ばすことでコライダーの大きさに左右されにくくする
    /// </summary>
    private static readonly float RayOffset = 0.5f;

    [Header("床を検知するためのRayの設定")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _rayDistance = 1.0f;
    [Header("うろうろする際に移動する距離の設定")]
    [SerializeField] private float _moveDistance = 3.0f;
    [Tooltip("移動距離にランダム性を持たせるかどうか")]
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
    /// 足元にRayを飛ばして当たった位置をうろうろの基準となる座標とする
    /// キャラクターが移動した際は基準の座標を変更する
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
            Debug.LogError("Rayがヒットしなかったので座標がセットできませんでした");
        }
    }

    /// <summary>
    /// 移動先の座標を空のオブジェクトのPositionにセットして移動先として返すことで
    /// うろうろで向かう先を移動中に変更できる
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
            // うろうろする範囲を表示する
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_wanderingCenterPos + Vector3.right * _moveDistance, 0.25f);
            Gizmos.DrawSphere(_wanderingCenterPos + Vector3.left * _moveDistance, 0.25f);
        }

        // うろうろの基準となる座標に向けたRayを表示する
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * RayOffset, Vector3.down * _rayDistance);
    }
}
