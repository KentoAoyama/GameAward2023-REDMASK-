using UnityEngine;

/// <summary>
/// デバッグ用
/// プレイヤーを中心に円を描く
/// </summary>
public class DebugCircleDrawer : MonoBehaviour
{
    [Tooltip("EnemyControllerクラスの_playerFireReactionDistanceと同じ値を設定する")]
    [SerializeField] private float _radius;
    [SerializeField] private Color _color;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
