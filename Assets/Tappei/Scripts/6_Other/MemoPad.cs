#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// ÉÅÉÇí†
/// </summary>
public class MemoPad : MonoBehaviour
{
    [TextArea(500, 500)]
    [SerializeField] string _memo;
}
#endif