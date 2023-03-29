// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class GoToStageNameAssigner : MonoBehaviour
{
    private Text _text = default;
    private IDisposable _disposer = null;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        _disposer = GameManager.Instance.StageSelectManager.GoToStageType.
            Subscribe(type => _text.text = $"Go to stage name : {type}");
    }
    private void OnDisable()
    {
        _disposer.Dispose();
    }
}
