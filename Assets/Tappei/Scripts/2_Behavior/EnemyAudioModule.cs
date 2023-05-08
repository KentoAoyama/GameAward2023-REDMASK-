using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵が使用する音を管理するクラス
/// EnemyController経由で使用される
/// </summary>
public class EnemyAudioModule
{
    // SEの再生
    // 撃破されたときに再生中の音は全部止める
    // マップ外で音をならないようにしたい
    // 武器にも音を鳴らす処理が必要

    // EnemyControllerに持たせるのでStateとEnemyController内からのみ呼び出せる
    // 各Behaviorから呼び出せないが大丈夫

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySE(string name)
    {
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", name);
    }

    public void StopSE()
    {

    }

    public void StopAll()
    {

    }
}
