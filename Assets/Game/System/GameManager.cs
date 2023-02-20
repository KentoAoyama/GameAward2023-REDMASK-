// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region Singleton
    private static GameManager _instance = new GameManager();
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }
    private GameManager(){}
    #endregion
}