// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ギャラリーの管理をするクラス
/// </summary>
[Serializable] // 保存用
public class GalleryManager
{
    [SerializeField] // 保存用
    private bool[] _openedID = new bool[_maxGalleryID]
    {
        false,false,false,false,
        false,false,false,false
    };

    private readonly string _saveFileName = "GalleryData";
    private const int _maxGalleryID = 8;

    private void Save()
    {
        SaveLoadManager.Save<GalleryManager>(this, _saveFileName);
    }
    public void Load()
    {
        var temp = SaveLoadManager.Load<GalleryManager>(_saveFileName);
        if (temp == null) return;
        this._openedID = temp._openedID;
    }

    public void SetOpenedID(bool value, int id)
    {
        try
        {
            _openedID[id] = value;
            Save();
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError($"範囲外が指定されました。値 : {id}");
            return;
        }
    }
    public bool IsOpenedID(int id)
    {
        try
        {
            return _openedID[id];
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError($"範囲外が指定されました。値 : {id}");
            return false;
        }
    }
}