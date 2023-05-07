using UnityEngine;

/// <summary>
/// Rayを飛ばしてオブジェクトを検知するクラス
/// MoveBehaviorクラスから使用される
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

    [Tooltip("このRayは左右の向きに応じて反転しないので注意")]
    [SerializeField] private RaySettings _footPosRaySettings;
    [Tooltip("このRayは左右の向きに応じて反転しないので注意")]
    [SerializeField] private RaySettings _floorRaySettings;
    [Tooltip("自身のコライダーとぶつからないように設定する必要がある")]
    [SerializeField] private RaySettings _enemyRaySettings;

    /// <summary>
    /// 足元(真下)に向かってRayを飛ばすことで床の上にいるかを判定する
    /// 床の上にいる場合はその座標を返すことでキャラクターの足元の基準になる座標を更新する
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
    /// 斜め前にRayを飛ばすことで進んだ先に床があるかを判定する
    /// </summary>
    public bool DetectFloorInFront(int dir, Transform transform)
    {
        // Rayの斜め下の向き加減
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
    /// 正面に向かってRayを飛ばすことで前方に敵がいるかどうかを判定する
    /// 現在使われていないが追加の機能を作る際に必要になりそうなので残している
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
