using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// このクラスを使用して視界内にプレイヤーがいるかどうかを検出する
/// スローモーション中かどうかに影響されない
/// </summary>
public class SightSensor : MonoBehaviour
{
    /// <summary>
    /// 一度に視界が検出できるオブジェクトの最大数
    /// ステージに大量の検出できるオブジェクトが存在する場合は増やす必要がある
    /// </summary>
    private static readonly int MaxDetected = 9;

    [Header("検出範囲の基準となるオブジェクト")]
    [Tooltip("視線上の障害物として検知してしまうので他のコライダーと被せないこと")]
    [SerializeField] private Transform _eyeTransform;
    [Header("検出するオブジェクトが属するレイヤー")]
    [SerializeField] private LayerMask _detectedLayerMask;

#if UNITY_EDITOR
    // 以下2つのフィールドはエディタのPlayモードで視界のギズモを表示するためのもの
    private float _radius;
    private float _angle;
#endif

    private Collider2D[] _detectedResults = new Collider2D[MaxDetected];

    /// <returns>
    /// プレイヤーが視界内にいる場合はプレイヤーとの距離を返す
    /// 視界内にいない場合は -1 が返る
    /// </returns>
    public float TryGetDistanceToPlayer(float radius, float maxAngle, bool isIgnoreObstacle = false)
    {
#if UNITY_EDITOR
        _radius = radius;
        _angle = maxAngle;
#endif

        Vector3 rayOrigin = _eyeTransform.position;

        // ヒットしなかった場合でも配列内の要素は削除されないので
        // ヒットしたオブジェクトの情報を返すように変更する場合は注意
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

            // 視界を遮るオブジェクト用のレイヤーがあれば、ターゲットまでのRayを飛ばして
            // 視界を遮るオブジェクトにヒットしたら視界に映らないという処理に変更出来る。
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
