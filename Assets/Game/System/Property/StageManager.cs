// 日本語対応
using Bullet;
using System.Collections.Generic;
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
    /// <summary> 
    /// どのようにステージを開始するかプレイヤーオブジェクトに教える用
    /// （プレイヤーの開始地点や弾の数をどこから割り当てるか判別する用。）
    /// （開始演出とかにも使用する、直前からの場合開始演出は再生しないなど）
    /// </summary>
    public StageStartMode StageStartMode { get; set; } = StageStartMode.NotSet;

    private IStoreableInChamber[] _checkPointCylinder = null;
    private Dictionary<BulletType, IReadOnlyReactiveProperty<int>> _checkPointGunBelt = null;

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
    /// <summary>
    /// プレイヤーがチェックポイントを起動したときに実行する関数
    /// </summary>
    /// <param name="position"> 復活座標 </param>
    /// <param name="cylinder"> チェックポイント起動時のシリンダーの状態 </param>
    /// <param name="gunBelt"> チェックポイント起動時のガンベルトの状態 </param>
    public void SetBulletsCount(IStoreableInChamber[] cylinder,
        Dictionary<BulletType, IReadOnlyReactiveProperty<int>> gunBelt)
    {
        // シリンダーの状態保存
        var cylinderResult = new IStoreableInChamber[_checkPointCylinder.Length];
        for (int i = 0; i < cylinderResult.Length; i++)
        {
            cylinderResult[i] = cylinder[i];
        }
        _checkPointCylinder = cylinderResult;

        // ガンベルトの状態保存
        var gunBeltResult = new Dictionary<BulletType, IReadOnlyReactiveProperty<int>>(gunBelt);
        gunBeltResult[BulletType.StandardBullet] =
            new ReactiveProperty<int>(gunBelt[BulletType.StandardBullet].Value);
        gunBeltResult[BulletType.PenetrateBullet] =
            new ReactiveProperty<int>(gunBelt[BulletType.PenetrateBullet].Value);
        gunBeltResult[BulletType.ReflectBullet] =
            new ReactiveProperty<int>(gunBelt[BulletType.ReflectBullet].Value);
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
