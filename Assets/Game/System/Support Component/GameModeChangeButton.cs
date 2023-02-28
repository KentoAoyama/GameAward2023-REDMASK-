// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeChangeButton : MonoBehaviour
{
    /// <summary>
    /// ボタンから呼び出すことを想定して作成したメソッド。
    /// ゲームモードを変更する。
    /// </summary>
    public void ChangeGameMode(GameMode gameMode)
    {
        GameManager.Instance.GameModeManager.ChangeGameMode(gameMode);
    }
}
