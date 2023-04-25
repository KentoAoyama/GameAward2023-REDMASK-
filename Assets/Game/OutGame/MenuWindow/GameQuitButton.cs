// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class GameQuitButton : MonoBehaviour
{
    [SerializeField]
    private Button _button = default;

    private void Awake()
    {
        _button.onClick.AddListener(GameQuit);
    }
    private void GameQuit()
    {
        // ゲームを終了する。
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
