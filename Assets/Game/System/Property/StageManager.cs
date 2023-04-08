// 日本語対応

using Bullet;
using System;
using UnityEngine;

/// <summary>
/// このクラスの目的をここに記す。
/// このクラスは、ステージの最初からやり直す、チェックポイントからやり直すを管理する為のクラス。
/// 直前に通過したチェックポイントの座標を覚えている。
/// チェックポイントを通過した際の弾の状態を覚えている。
/// チェックポイントを通過した際の敵の状態を覚えている。
/// </summary>
public class StageManager
{
    /// <summary> 最後に通過したチェックポイントの座標 </summary>
    public Vector2 LastCheckPointPosition { get; set; } = default;
    /// <summary> 
    /// どのようにステージを開始するかプレイヤーオブジェクトに教える用
    /// （プレイヤーの開始地点や弾の数をどこから割り当てるか判別する用。）
    /// （開始演出とかにも使用する、直前からの場合開始演出は再生しないなど）
    /// </summary>
    public StageStartMode StageStartMode { get; set; } = StageStartMode.NotSet;

    private BulletType[] _cylinder = default;
    private BulletType[] _gunBelt = default;
    private CachedEnemy[] cachedEnemies = default;

    public BulletType[] Cylinder => _cylinder;
    public BulletType[] GunBelt => _gunBelt;

    /// <summary>
    /// プレイヤーがチェックポイントを起動したときに実行する関数
    /// </summary>
    /// <param name="position"> 復活座標 </param>
    /// <param name="cylinder"> チェックポイント起動時のシリンダーの状態 </param>
    /// <param name="gunBelt"> チェックポイント起動時のガンベルトの状態 </param>
    public void TouchCheckPoint(Vector2 position,
        BulletType[] cylinder, BulletType gunBelt)
    {
        cachedEnemies = new CachedEnemy[GameManager.Instance.EnemyRegister.Enemies.Count];
        var index = 0;
        foreach (var e in GameManager.Instance.EnemyRegister.Enemies)
        {
            cachedEnemies[index] = new CachedEnemy(e.transform.position, e.EnemyType);
            index++;
        }
    }
    /// <summary>
    /// シーンに敵を配置する
    /// </summary>
    private void PlaceEnemies()
    {

    }
}
/// <summary>
/// 一時的にエネミーの情報を保存する用
/// </summary>
public struct CachedEnemy
{
    public CachedEnemy(Vector2 position, EnemyType type)
    {
        _position = position;
        _enemyType = type;
    }
    public Vector2 _position;
    public EnemyType _enemyType;
}
/// <summary>
/// エネミーの種類を表現する列挙型
/// </summary>
[Serializable]
public enum EnemyType
{
    Melee, Drawn, Range, Shield
}
/// <summary>
/// ステージをどのように開始するか表現する列挙体
/// </summary>
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
