// 日本語対応

using UnityEngine;

public class StageManager
{
    /// <summary> 最後に通過したチェックポイントの座標 </summary>
    public Vector2 LastCheckPoint { get; set; } = default;
    /// <summary> ステージどのようにステージを開始するかプレイヤーオブジェクトに教える用 </summary>
    public StageStartMode StageStartMode { get; set; } = StageStartMode.NotSet;
}
public enum StageStartMode
{
    NotSet,
    /// <summary>
    /// 最初からやり直す
    /// </summary>
    FromTheBeginning,
    /// <summary>
    /// 直前からやり直す
    /// </summary>
    JustBefore
}
