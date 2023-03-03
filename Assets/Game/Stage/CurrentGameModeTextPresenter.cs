// 日本語対応
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CurrentGameModeTextPresenter : MonoBehaviour
{
    [SerializeField]
    private Text _targetText = default;

    private IDisposable _disposer = null;

    private void OnEnable()
    {
        _disposer = GameManager.Instance.GameModeManager.CurrentGameMode.
            Subscribe(newGamemode => _targetText.text = $"Current Game mode : {newGamemode}");
    }
    private void OnDisable()
    {
        _disposer.Dispose();
    }
}
