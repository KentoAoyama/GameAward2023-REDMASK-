// 日本語対応
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerformanceEventController : MonoBehaviour
{
    [SerializeField]
    private PerformanceEvent[] _performanceEvents = default;
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    private async void Awake()
    {
        for (int i = 0; i < _performanceEvents.Length; i++)
        {
            await _performanceEvents[i].Execute();
        }
        SceneManager.LoadScene(_nextSceneName);
    }
}
