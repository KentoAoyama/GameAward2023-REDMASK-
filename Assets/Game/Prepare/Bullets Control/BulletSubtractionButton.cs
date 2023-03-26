// 日本語対応
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 弾を減算する用のボタン
/// </summary>
public class BulletSubtractionButton
    : MonoBehaviour
{
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;
    [SerializeField]
    private int _index = default;
    [SerializeField]
    private StorageSiteType _storageSiteType = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(BulletSubtraction);
    }
    private void BulletSubtraction()
    {
        _bulletPrepareControl.PullBullet(_storageSiteType, _index);
    }
}
