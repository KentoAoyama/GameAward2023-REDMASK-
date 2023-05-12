// 日本語対応
using Bullet;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 弾の数を覚えておくクラス
/// </summary>
[Serializable] // 保存するクラス
public class BulletsCountManager
{
    /// <summary>
    /// アジトにある弾の数
    /// </summary>
    private Dictionary<BulletType, IntReactiveProperty> _bulletCountHome = new Dictionary<BulletType, IntReactiveProperty>() {
            { BulletType.StandardBullet, new IntReactiveProperty(0) },
            { BulletType.PenetrateBullet, new IntReactiveProperty(0) },
            { BulletType.ReflectBullet, new IntReactiveProperty(0) } };
    /// <summary>
    /// ステージ内で所持している弾の数
    /// </summary>
    private Dictionary<BulletType, IntReactiveProperty> _bulletCountStage = new Dictionary<BulletType, IntReactiveProperty>() {
            { BulletType.StandardBullet, new IntReactiveProperty(0) },
            { BulletType.PenetrateBullet, new IntReactiveProperty(0) },
            { BulletType.ReflectBullet, new IntReactiveProperty(0) } };
    /// <summary>
    /// シリンダーの状態を表現する値
    /// </summary>
    private BulletType[] _cylinder = new BulletType[] {
        BulletType.NotSet, BulletType.NotSet, BulletType.NotSet,
        BulletType.NotSet, BulletType.NotSet, BulletType.NotSet};

    /// <summary>
    /// 保存するフィールド
    /// </summary>
    [SerializeField]
    private int[] _bulletCountHomeSaveData;

    public Dictionary<BulletType, IntReactiveProperty> BulletCountHome => _bulletCountHome;
    public Dictionary<BulletType, IntReactiveProperty> BulletCountStage => _bulletCountStage;

    public BulletsCountManager()
    {
        // 各弾の初期所持数を設定する
        _bulletCountHome[BulletType.StandardBullet].Value = 20;
        _bulletCountHome[BulletType.PenetrateBullet].Value = 8;
        _bulletCountHome[BulletType.ReflectBullet].Value = 12;
    }

    public void Reset()
    {
        _bulletCountHome[BulletType.StandardBullet].Value = 20;
        _bulletCountHome[BulletType.PenetrateBullet].Value = 8;
        _bulletCountHome[BulletType.ReflectBullet].Value = 12;
    }
    public BulletType[] Cylinder
    {
        get => _cylinder; set => _cylinder = value;
    }

    private string _saveFileName = "BulletsCount";

    public void Save()
    {
        _bulletCountHomeSaveData = new int[3];
        _bulletCountHomeSaveData[0] = _bulletCountHome[BulletType.StandardBullet].Value;
        _bulletCountHomeSaveData[1] = _bulletCountHome[BulletType.PenetrateBullet].Value;
        _bulletCountHomeSaveData[2] = _bulletCountHome[BulletType.ReflectBullet].Value;
        SaveLoadManager.Save<BulletsCountManager>(this, _saveFileName);
    }
    public void Load()
    {
        var temp = SaveLoadManager.Load<BulletsCountManager>(_saveFileName);
        if (temp == null || temp._bulletCountHomeSaveData == null) return; // 読み込みに失敗した場合は処理しない。
        // Debug.Log(temp._bulletCountHomeSaveData == null);
        _bulletCountHome[BulletType.StandardBullet].Value = temp._bulletCountHomeSaveData[0];
        _bulletCountHome[BulletType.PenetrateBullet].Value = temp._bulletCountHomeSaveData[1];
        _bulletCountHome[BulletType.ReflectBullet].Value = temp._bulletCountHomeSaveData[2];
    }
    public void Clear()
    {
        // ステージの弾の数を初期値に戻す
        _bulletCountStage[BulletType.StandardBullet].Value = 0;
        _bulletCountStage[BulletType.PenetrateBullet].Value = 0;
        _bulletCountStage[BulletType.ReflectBullet].Value = 0;
        // シリンダーの状態を初期値に戻す
        for (int i = 0; i < _cylinder.Length; i++)
        {
            _cylinder[i] = BulletType.NotSet;
        }
    }
}