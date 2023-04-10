// 日本語対応
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// インスペクタウィンドウでポーズとリジュームを割り当てる用のコンポーネント
/// </summary>
public class PauseComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onPause = default;
    [SerializeField]
    private UnityEvent _onResume = default;

    public void ExecutePause()
    {
        GameManager.Instance.PauseManager.ExecutePause();
    }
    public void ExecuteResume()
    {
        GameManager.Instance.PauseManager.ExecuteResume();
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.RegisterInspectorPauseAndResume(_onPause, _onResume);
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.LiftInspectorPauseAndResume();
    }
}
