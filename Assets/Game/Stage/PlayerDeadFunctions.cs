// 日本語対応
using UnityEngine;

/// <summary>
/// プレイヤー死亡時に表示するウィンドウのボタンに割り当てる機能をまとめて持つコンポーネント
/// </summary>
public class PlayerDeadFunctions : MonoBehaviour
{
    /// <summary>
    /// ステージの最初からやり直す
    /// </summary>
    public void FromTheBeginning()
    {
        GameManager.Instance.StageManager.StageStartMode = StageStartMode.FromTheBeginning;
    }
    /// <summary>
    /// 直前からやり直す
    /// </summary>
    public void JustBefore()
    {
        GameManager.Instance.StageManager.StageStartMode = StageStartMode.JustBefore;
    }
}
