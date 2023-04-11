// 日本語対応

using Bullet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    private IStoreableInChamber[] _checkPointCylinder = null;
    private Dictionary<BulletType, IReadOnlyReactiveProperty<int>> _checkPointGunBelt = null;
    private CachedEnemy[] cachedEnemies = null;

    /// <summary>
    /// チェックポイントのシリンダーの状態を返す
    /// </summary>
    public IStoreableInChamber[] CheckPointCylinder
    {
        get
        {
            var result = new IStoreableInChamber[_checkPointCylinder.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = _checkPointCylinder[i];
            }
            return result;
        }

    }
    /// <summary>
    /// チェックポイントのガンベルトの状態を返す
    /// </summary>
    public Dictionary<BulletType, IReadOnlyReactiveProperty<int>> CheckPointGunBelt
    {
        get
        {
            var result = new Dictionary<BulletType, IReadOnlyReactiveProperty<int>>(_checkPointGunBelt);
            result[BulletType.StandardBullet] =
                new ReactiveProperty<int>(_checkPointGunBelt[BulletType.StandardBullet].Value);
            result[BulletType.PenetrateBullet] =
                new ReactiveProperty<int>(_checkPointGunBelt[BulletType.PenetrateBullet].Value);
            result[BulletType.ReflectBullet] =
                new ReactiveProperty<int>(_checkPointGunBelt[BulletType.ReflectBullet].Value);
            return null;
        }
    }
    public bool IsTouchCheckPoint { get; set; } = false;
    /// <summary>
    /// プレイヤーがチェックポイントを起動したときに実行する関数
    /// </summary>
    /// <param name="position"> 復活座標 </param>
    /// <param name="cylinder"> チェックポイント起動時のシリンダーの状態 </param>
    /// <param name="gunBelt"> チェックポイント起動時のガンベルトの状態 </param>
    public void TouchCheckPoint(Vector2 position,
        IStoreableInChamber[] cylinder, Dictionary<BulletType, IReadOnlyReactiveProperty<int>> gunBelt)
    {
        IsTouchCheckPoint = true;
        LastCheckPointPosition = position;
        // エネミーの状態保存
        cachedEnemies = new CachedEnemy[GameManager.Instance.EnemyRegister.Enemies.Count];
        var index = 0;
        foreach (var e in GameManager.Instance.EnemyRegister.Enemies)
        {
            cachedEnemies[index] = new CachedEnemy(e.transform.position, e.EnemyType);
            index++;
        }
        // シリンダーの状態保存
        _checkPointCylinder = cylinder;
        // ガンベルトの状態保存
        _checkPointGunBelt = gunBelt;
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
