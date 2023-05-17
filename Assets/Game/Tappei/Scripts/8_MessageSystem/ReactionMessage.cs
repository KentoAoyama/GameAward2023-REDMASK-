using UnityEngine;

/// <summary>
/// プレイヤーの銃撃に敵が反応する為の構造体
/// これをメッセージングでやり取りする
/// </summary>
public readonly struct ReactionMessage
{
    public ReactionMessage(Vector3 pos)
    {
        Pos = pos;
    }

    public Vector3 Pos { get; }
}
