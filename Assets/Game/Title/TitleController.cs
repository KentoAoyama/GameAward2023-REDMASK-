// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// タイトル画面を制御するクラス
/// </summary>
public class TitleController : MonoBehaviour
{
    [Tooltip("タイトル画面起動時に不要なオブジェクトを割り当ててください"), SerializeField]
    private GameObject[] _firstNotNeededObject = default;

    private TitleState _currentState = TitleState.Title;
    private GameObject _lastSelectedObject = default;
    public TitleState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

    private void Start()
    {
        GameManager.Instance.AudioManager.Load();
        GameManager.Instance.AudioManager.PlayBGM("CueSheet_Gun", "BGM_Title");
        GameManager.Instance.GalleryManager.Load();

        Setup();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
           EventSystem.current.SetSelectedGameObject(_lastSelectedObject);
        }  
        if (EventSystem.current.currentSelectedGameObject != _lastSelectedObject)
        {
            _lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }
    }

    /// <summary>
    /// タイトルシーンのセットアップ処理
    /// </summary>
    private void Setup()
    {
        StartDeactivateObjects();
    }

    /// <summary>
    /// シーン起動時に必要ないオブジェクトを非アクティブにする。
    /// </summary>
    private void StartDeactivateObjects()
    {
        foreach (var e in _firstNotNeededObject)
        {
            e.SetActive(false);
        }
    }

    public enum TitleState
    {
        Title,
        Menu
    }
}
