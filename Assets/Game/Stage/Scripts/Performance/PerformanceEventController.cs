// 日本語対応
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class PerformanceEventController : MonoBehaviour
{
    [Header("射撃後までのアニメーション")]
    [SerializeField]
    private PerformanceEvent[] _performanceEvents = default;
    [Header("射撃後に行う処理")]
    [SerializeField]
    public PerformanceEvent[] _afterFirePerformanceEvents = default;
    [SceneName, SerializeField]
    private string _nextSceneName = default;

    private int _currentIndex = 0;
    private bool _isAfter = false;

    public event Action OnComplete = default;

    private async void Awake()
    {
        for (_currentIndex = 0; _currentIndex < _performanceEvents.Length; _currentIndex++)
        {
            await _performanceEvents[_currentIndex].Execute();
            if (_performanceEvents[_currentIndex].IsFire) break;
        }
        _isAfter = true;
        for (_currentIndex = 0; _currentIndex < _afterFirePerformanceEvents.Length; _currentIndex++)
        {
            await _afterFirePerformanceEvents[_currentIndex].Execute();
        }

        OnComplete?.Invoke();

        SceneManager.LoadScene(_nextSceneName);
    }

    private void Update()
    {
        if (_isAfter && _currentIndex > -1 && _currentIndex < _afterFirePerformanceEvents.Length)
        {
            _afterFirePerformanceEvents[_currentIndex].Update();
        }
        else if(_currentIndex > -1 && _currentIndex < _performanceEvents.Length)
        {
            _performanceEvents[_currentIndex].Update();
        }
    }

    /// <summary>
    /// 発砲ボタンが押下されるのを待つ。（左クリックかゲームパッドのRightShoulder）
    /// </summary>
    /// <returns></returns>
    private bool WaitInput()
    {
        if (Mouse.current != null)
        {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }
        if (Gamepad.current != null)
        {
            return Gamepad.current.rightShoulder.wasPressedThisFrame;
        }
        return false;
    }
}
