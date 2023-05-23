// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Bullet;
using System;

/// <summary>
/// 準備画面用 <br/>
/// 弾の数を表示する用のクラス
/// </summary>
public class PrepareBulletsNumberPresenter : MonoBehaviour
{
    [SerializeField]
    private Text _standardBulletNum;
    [SerializeField]
    private Text _penetrateBulletNum;
    [SerializeField]
    private Text _reflectBulletNum;

    private IDisposable _standardDisposable = null;
    private IDisposable _penetrateDisposable = null;
    private IDisposable _reflectDisposable = null;

    private void OnEnable()
    {
        _standardDisposable =
        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.StandardBullet].Subscribe(value =>
            _standardBulletNum.text = $"{value}");

        _penetrateDisposable =
        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.PenetrateBullet].Subscribe(value =>
            _penetrateBulletNum.text = $"{value}");

        _reflectDisposable =
        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.ReflectBullet].Subscribe(value =>
            _reflectBulletNum.text = $"{value}");
    }
    private void OnDisable()
    {
        _standardDisposable.Dispose();
        _penetrateDisposable.Dispose();
        _reflectDisposable.Dispose();
    }
}
