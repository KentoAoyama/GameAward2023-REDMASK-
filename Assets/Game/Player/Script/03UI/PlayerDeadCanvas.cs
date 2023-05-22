// 日本語対応
using UnityEngine;

public class PlayerDeadCanvas : MonoBehaviour
{
    [SerializeField]
    private MenuWindowStartUpButton button = default;

    private void OnEnable()
    {
        button.enabled = false;
        GameManager.Instance.PauseManager.ExecutePause();
    }
    private void OnDisable()
    {
        button.enabled = true;
        GameManager.Instance.PauseManager.ExecuteResume();
    }
}
