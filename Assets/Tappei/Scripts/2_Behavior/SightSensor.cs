using UnityEngine;

/// <summary>
/// このクラスを使用して視界内にプレイヤーがいるかどうかを検出する
/// スローモーション中かどうかに影響されない
/// </summary>
public class SightSensor : MonoBehaviour
{
    /// <summary>プレイヤーが視界内にいない場合に返る値</summary>
    public static readonly int PlayerOutSight = -1;

    /// <summary>
    /// 一度に視界が検出できるオブジェクトの最大数
    /// ステージに大量の検出できるオブジェクトが存在する場合は増やす必要がある
    /// </summary>
    private static readonly int MaxDetected = 9;

    [Header("検出範囲の基準となるオブジェクト")]
    [Tooltip("視線上の障害物として検知してしまうので他のコライダーと被せないこと")]
    [SerializeField] private Transform _eyeTransform;
    [Header("検出するオブジェクトが属するレイヤー")]
    [SerializeField] private LayerMask _playerLayerMask;
    [Tooltip("視界を遮るオブジェクトのレイヤー")]
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Collider2D[] _detectedResults = new Collider2D[MaxDetected];

#if UNITY_EDITOR
    /// <summary>EnemyControllerでギズモに表示する用途で使っている</summary>
    public Transform EyeTransform => _eyeTransform;
#endif

    /// <returns>
    /// プレイヤーが視界内にいる場合はプレイヤーとの距離を返す
    /// 視界内にいない場合は PlayerOutSight が返る
    /// </returns>
    public float TryGetDistanceToPlayer(float radius, float maxAngle, bool isIgnoreObstacle = false)
    {
        Vector3 rayOrigin = _eyeTransform.position;

        // ヒットしなかった場合でも配列内の要素は削除されないので
        // ヒットしたオブジェクトの情報を返すように変更する場合は注意
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

            // 視界を遮るオブジェクト用のレイヤーがあれば、ターゲットまでのRayを飛ばして
            // 視界を遮るオブジェクトにヒットしたら視界に映らないという処理に変更出来る。
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
