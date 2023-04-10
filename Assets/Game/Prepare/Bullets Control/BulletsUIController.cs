// 日本語対応
using Bullet;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BulletsUIController : MonoBehaviour
{
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;

    [Header("各ボタンのスプライト")]
    [SerializeField]
    private Image[] _cylinder = default;
    [SerializeField]
    private Image[] _gunBelt = default;

    [Header("シリンダー用 弾画像")]
    [SerializeField]
    private Sprite _standardBulletCylinder = default;
    [SerializeField]
    private Sprite _penetrateBulletCylinder = default;
    [SerializeField]
    private Sprite _reflectBulletCylinder = default;

    [Header("ガンベルト用 弾画像")]
    [SerializeField]
    private Sprite _standardBulletGunBelt = default;
    [SerializeField]
    private Sprite _penetrateBulletGunBelt = default;
    [SerializeField]
    private Sprite _reflectBulletGunBelt = default;

    private void Awake()
    {
        // シリンダー UI更新処理を登録
        for (int i = 0; i < _bulletPrepareControl.Cylinder.Length; i++)
        {
            int index = i;
            _bulletPrepareControl.Cylinder[index].Subscribe(type => CylinderImageUpdate(type, index));
        }
        // ガンベルト UI更新処理を登録
        for (int i = 0; i < _bulletPrepareControl.GunBelt.Length; i++)
        {
            int index = i;
            _bulletPrepareControl.GunBelt[index].Subscribe(type => GunBeltImageUpdate(type, index));
        }
    }

    private void CylinderImageUpdate(BulletType bulletType, int index)
    {
        try
        {
            switch (bulletType)
            {
                case BulletType.StandardBullet:
                    _cylinder[index].color = Color.white;
                    _cylinder[index].sprite = _standardBulletCylinder;
                    break;
                case BulletType.PenetrateBullet:
                    _cylinder[index].color = Color.white;
                    _cylinder[index].sprite = _penetrateBulletCylinder;
                    break;
                case BulletType.ReflectBullet:
                    _cylinder[index].color = Color.white;
                    _cylinder[index].sprite = _reflectBulletCylinder;
                    break;
                default:
                    _cylinder[index].color = Color.clear;
                    _cylinder[index].sprite = null;
                    break;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError($"範囲外が指定されました。 値 :{index}");
        }
    }
    private void GunBeltImageUpdate(BulletType bulletType, int index)
    {
        try
        {
            switch (bulletType)
            {
                case BulletType.StandardBullet:
                    _gunBelt[index].color = Color.white;
                    _gunBelt[index].sprite = _standardBulletGunBelt;
                    break;
                case BulletType.PenetrateBullet:
                    _gunBelt[index].color = Color.white;
                    _gunBelt[index].sprite = _penetrateBulletGunBelt;
                    break;
                case BulletType.ReflectBullet:
                    _gunBelt[index].color = Color.white;
                    _gunBelt[index].sprite = _reflectBulletGunBelt;
                    break;
                default:
                    _gunBelt[index].color = Color.clear;
                    _gunBelt[index].sprite = null;
                    break;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError($"範囲外が指定されました。 値 :{index}");
        }
    }
}