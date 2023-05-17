using UnityEngine;

/// <summary>
/// 各Enemyに割り当ててあるSOのパラメータをGizmo上に表示するクラス
/// </summary>
public class ParamsGizmoDrawer : MonoBehaviour
{
    [SerializeField] private Transform _eye;
    
    private EnemyController _controller;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying && _controller != null)
        {
            DrawSightArc(new Color32(0, 0, 255, 64), _controller.SightRadius);
            DrawSightArc(new Color32(255, 0, 0, 64), _controller.Params.AttackRange);
        }
    }

    private void DrawSightArc(Color32 color, float radius)
    {
        Vector3 dir = Quaternion.Euler(0, 0, -_controller.SightAngle / 2) * _eye.right;

        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidArc(_eye.position, Vector3.forward, 
            dir, _controller.SightAngle, radius);
    }
#endif
}
