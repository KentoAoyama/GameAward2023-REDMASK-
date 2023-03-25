// 日本語対応
using Bullet;
using System;
using System.Linq;
using UnityEngine;
using UniRx;

/// <summary>
/// 準備画面上での弾を制御するクラス
/// </summary>
public class BulletPrepareControl : MonoBehaviour
{
    /// <summary>
    /// シリンダーの状態を表す値
    /// </summary>
    private ReactiveProperty<BulletType>[] _cylinder ={
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
    };
    /// <summary>
    /// ガンベルトの状態を表す値
    /// </summary>
    private ReactiveProperty<BulletType>[] _gunBelt = {
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
        new ReactiveProperty<BulletType>(BulletType.NotSet),
    };

    public IReactiveProperty<BulletType>[] Cylinder => _cylinder;
    public IReactiveProperty<BulletType>[] GunBelt => _gunBelt;

    /// <summary>
    /// 決められた順番に弾を装填する <br/>
    /// 参考 : https://discord.com/channels/1069273491297271849/1070547785369255996/1088332634402328606
    /// </summary>
    /// <param name="bulletType"> 格納する弾の種類 </param>
    /// <returns> 
    /// 格納に成功したら (格納場所番号, 格納場所種類) を返す。 
    /// 格納に失敗したら（-1, StorageSiteType.Error）を返す。
    /// </returns>
    public (int, StorageSiteType) PushBullet(BulletType bulletType)
    {
        // 空のチェンバーを探す
        for (int i = 0; i < _cylinder.Length; i++)
        {
            if (_cylinder[i].Value == BulletType.NotSet)
            {
                _cylinder[i].Value = bulletType;
                return (i, StorageSiteType.Cylinder);
            }
        }
        // 空のガンベルトを探す
        for (int i = 0; i < _gunBelt.Length; i++)
        {
            if (_gunBelt[i].Value == BulletType.NotSet)
            {
                _gunBelt[i].Value = bulletType;
                return (i, StorageSiteType.GunBelt);
            }
        }
        return (-1, StorageSiteType.Error);
    }
    /// <summary>
    /// 指定の場所から 弾を引き抜く。
    /// </summary>
    /// <param name="storageSiteType">  格納している場所の種類（シリンダーかガンベルトか） </param>
    /// <param name="index"> 格納している場所を表現する番号 </param>
    /// <returns> 成功したら true, 失敗したら falseを返す。 </returns>
    public bool PullBullet(StorageSiteType storageSiteType, int index)
    {
        try
        {
            if (storageSiteType == StorageSiteType.Cylinder)
            {
                _cylinder[index].Value = BulletType.NotSet;
                return true;
            }
            if (storageSiteType == StorageSiteType.GunBelt)
            {
                _gunBelt[index].Value = BulletType.NotSet;
                return true;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError($"範囲外が指定されました！\n" +
                $"種類 :{storageSiteType}, インデックス :{index}");
        }
        return false;
    }
    /// <summary>
    /// 準備画面で行った変更をゲームマネージャーに保存する
    /// </summary>
    public void AssignBulletsCount()
    {
        // 全ての種類を走査する
        foreach (var e in GameManager.Instance.BulletsCountManager.BulletCountStage)
        {
            var type = e.Key;
            var count = 0;

            // 現在走査している種類と合致する,ガンベルトにある弾の総数を取得する。
            for (int i = 0; i < _gunBelt.Length; i++)
            {
                if (_gunBelt[i].Value == type) count++;
            }
            // ガンベルトの状態を保存
            GameManager.Instance.BulletsCountManager.BulletCountStage[type] = count;

            // 現在走査している種類と合致する,シリンダーにある弾の総数を取得する。
            for (int i = 0; i < _cylinder.Length; i++)
            {
                if (_cylinder[i].Value == type) count++;
            }
            // アジトの弾の数を更新
            GameManager.Instance.BulletsCountManager.BulletCountHome[type] -= count;
        }
        BulletType[] bullets = new BulletType[_cylinder.Length];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = _cylinder[i].Value;
        }
        // シリンダーの状態を保存
        GameManager.Instance.BulletsCountManager.Cylinder = bullets;
    }
}

/// <summary>
/// 弾の格納場所の種類
/// </summary>
[Serializable]
public enum StorageSiteType
{
    GunBelt, Cylinder, Error
}