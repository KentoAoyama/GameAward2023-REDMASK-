// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using Bullet;

/// <summary>
/// 弾の数を設定する用のコンポーネント
/// </summary>
public class SetBulletsNumberButton : MonoBehaviour
{
    [SerializeField]
    private int _standardBulletNumber = 10;
    [SerializeField]
    private int _penetrateBulletNumber = 10;
    [SerializeField]
    private int _reflectBulletNumber = 10;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SetBulletsNumber);
    }
    private void SetBulletsNumber()
    {
        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.StandardBullet].Value =
                _standardBulletNumber;

        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.PenetrateBullet].Value =
                _penetrateBulletNumber;

        GameManager.Instance.BulletsCountManager.
            BulletCountHome[BulletType.ReflectBullet].Value =
                _reflectBulletNumber;
    }
}
