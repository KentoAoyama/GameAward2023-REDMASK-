// 日本語対応
using UnityEngine;

public class PlayerDeadCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.PauseManager.ExecutePause();
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.ExecuteResume();
    }
}
