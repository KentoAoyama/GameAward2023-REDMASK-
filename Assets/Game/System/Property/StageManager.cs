// 日本語対応
using Bullet;
using System.Collections.Generic;
using UniRx;
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
    /// <summary> 
    /// どのようにステージを開始するかプレイヤーオブジェクトに教える用
    /// （プレイヤーの開始地点や弾の数をどこから割り当てるか判別する用。）
    /// （開始演出とかにも使用する、直前からの場合開始演出は再生しないなど）
    /// </summary>
    public StageStartMode StageStartMode { get; set; } = StageStartMode.NotSet;

    private IStoreableInChamber[] _checkPointCylinder = null;
    private Dictionary<BulletType, IReadOnlyReactiveProperty<int>> _checkPointGunBelt = null;

    /// <summary>
    /// チェックポイントのシリンダーの状態
    /// </summary>
    public IStoreableInChamber[] CheckPointCylinder { get => _checkPointCylinder; }
    /// <summary>
    /// チェックポイントのガンベルトの状態
    /// </summary>
    public Dictionary<BulletType, IReadOnlyReactiveProperty<int>> CheckPointGunBelt { get => _checkPointGunBelt; }

    public int CylinderIndex { get; set; } = 0;

    public void Clear()
    {
        _checkPointCylinder = null;
        _checkPointGunBelt = null;
        CylinderIndex = 0;
    }
    /// <summary>
    /// チェックポイント時の弾の数を保存する
    /// </summary>
    /// <param name="cylinder"> シリンダーの数 </param>
    /// <param name="gunBelt"> ガンベルトの数 </param>
    public void SetCheckPointBulletsCount(IStoreableInChamber[] cylinder,
        Dictionary<BulletType, IReadOnlyReactiveProperty<int>> gunBelt)
    {
        // シリンダーの状態保存
        var cylinderResult = new IStoreableInChamber[cylinder.Length];
        for (int i = 0; i < cylinderResult.Length; i++)
        {
            cylinderResult[i] = cylinder[i];
        }
        _checkPointCylinder = cylinderResult;

        // ガンベルトの状態保存
        var gunBeltResult = new Dictionary<BulletType, IReadOnlyReactiveProperty<int>>();
        gunBeltResult.Add(BulletType.StandardBullet,
            new ReactiveProperty<int>(gunBelt[BulletType.StandardBullet].Value));
        gunBeltResult.Add(BulletType.PenetrateBullet,
            new ReactiveProperty<int>(gunBelt[BulletType.PenetrateBullet].Value));
        gunBeltResult.Add(BulletType.ReflectBullet,
            new ReactiveProperty<int>(gunBelt[BulletType.ReflectBullet].Value));
        _checkPointGunBelt = gunBeltResult;
    }
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
