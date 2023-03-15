using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// このクラスを使用して視界内にプレイヤーがいるかどうかを検出する
/// この機能はスローモーション中かどうかに影響されない
/// </summary>
public class SightSensor : MonoBehaviour
{
    static readonly int MaxDetected = 9;

    [Header("検出範囲の基準となるTransform")]
    [Tooltip("視線上の障害物として検知してしまうので他のコライダーと被せないこと")]
    [SerializeField] Transform _eyeTransform;
    [Header("検出範囲の設定")]
    [SerializeField] float _radius;
    [SerializeField] float _angle;
    [Header("検出するオブジェクトが属するレイヤー")]
    [SerializeField] LayerMask _detectedLayerMask;
    [Header("間に障害物があった場合に無視をする")]
    [SerializeField] bool _isIgnoreObstacle;

    Collider2D[] _detectedResults = new Collider2D[MaxDetected];

    /// <summary>プレイヤーが検出範囲内にいるかどうかを返す</summary>
    public bool IsDetected()
    {
        Vector3 rayOrigin = _eyeTransform.position;

        // ヒットしなかった場合でも配列内の要素は削除されないので
        // ヒットしたオブジェクトの情報を返すように変更する場合は注意
        int hitCount = Physics2D.OverlapCircleNonAlloc(rayOrigin, _radius, _detectedResults, _detectedLayerMask);
        if (hitCount == 0) return false;

        foreach (Collider2D detectedCollider in _detectedResults)
        {
            if (detectedCollider == null) break;

            Vector3 targetPos = detectedCollider.transform.position;
            Vector3 targetDir = Vector3.Normalize(targetPos - rayOrigin);
            float angle = Vector3.Angle(targetDir, _eyeTransform.right);

            if (angle > _angle / 2) continue;

            float distance = Vector3.Distance(rayOrigin, targetPos);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, targetDir, distance);

            if (_isIgnoreObstacle) return true;

            // 視界を遮るオブジェクト用のレイヤーがあれば、ターゲットまでのRayを飛ばして
            // 視界を遮るオブジェクトにヒットしたら視界に映らないという処理に変更出来る。
            bool isSightable = hit.collider.GetInstanceID() == detectedCollider.GetInstanceID();

#if UNITY_EDITOR
            Color color = isSightable ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, targetDir * distance, color);
#endif
            if (isSightable) return true;
        }

        return false;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (_eyeTransform != null)
        {
            DrawSearchArea();
        }
    }

    void DrawSearchArea()
    {
        Handles.color = new Color32(0, 0, 255, 64);
        Vector3 dir = Quaternion.Euler(0, 0, -_angle / 2) * _eyeTransform.right;
        Handles.DrawSolidArc(_eyeTransform.position, Vector3.forward, dir, _angle, _radius);
    }
#endif
}
