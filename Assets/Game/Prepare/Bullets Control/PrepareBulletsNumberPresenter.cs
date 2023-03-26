// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Bullet;

/// <summary>
/// 
/// </summary>
public class PrepareBulletsNumberPresenter : MonoBehaviour
{
    [SerializeField]
    private Text _standardBulletNum;
    [SerializeField]
    private Text _penetrateBulletNum;
    [SerializeField]
    private Text _reflectBulletNum;

    private void Awake()
    {
        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.StandardBullet].Subscribe(value =>
            _standardBulletNum.text = $"標準弾の残りの数\n{value}");

        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.PenetrateBullet].Subscribe(value =>
            _penetrateBulletNum.text = $"貫通弾の残りの数\n{value}");

        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.ReflectBullet].Subscribe(value =>
            _reflectBulletNum.text = $"反射" +
            $"弾の残りの数\n{value}");
    }
}
