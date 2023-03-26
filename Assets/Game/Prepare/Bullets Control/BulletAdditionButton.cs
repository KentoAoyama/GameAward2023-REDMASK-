// 日本語対応
using Bullet;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 弾を加算する用のボタン
/// </summary>
public class BulletAdditionButton : MonoBehaviour
{
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;
    [SerializeField]
    private BulletType _bulletType = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(BulletAddition);
    }
    private void BulletAddition()
    {
        _bulletPrepareControl.PushBullet(_bulletType);
    }
}
