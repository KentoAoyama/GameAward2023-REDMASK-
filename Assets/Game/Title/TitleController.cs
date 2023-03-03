// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面を制御するクラス
/// </summary>
public class TitleController : MonoBehaviour
{
    [Tooltip("タイトル画面起動時に不要なオブジェクトを割り当ててください"), SerializeField]
    private GameObject[] _firstNotNeededObject = default;

    private void Start()
    {
        Setup();
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
}
