// 日本語対応
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("蘇生座標")]
    private Transform _revivePosition = default;
    public Transform RevivePosition => _revivePosition;

    /// <summary>
    /// このチェックポイントが生きているかどうか
    /// </summary>
    private bool _isAlive = true;

    public bool IsAlive { get; set; } = true;
}
