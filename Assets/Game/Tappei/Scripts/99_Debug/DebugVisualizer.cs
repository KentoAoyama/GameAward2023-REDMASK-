using UnityEngine;

public class DebugVisualizer : MonoBehaviour
{
    static DebugVisualizer _instance;

    [SerializeField] private GameObject _prefab;

    public static DebugVisualizer Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void Visualize(Vector3 pos)
    {
        GameObject instance = Instantiate(_prefab, pos, Quaternion.identity);
    }
}
