// 日本語対応
using Bullet;
using UnityEngine;

public class BulletsCountTest : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private bool _useTestValue = false;
    [SerializeField]
    private int _testStandardBulletCount = 5;
    [SerializeField]
    private int _testPenetrateBulletCount = 5;
    [SerializeField]
    private int _testReflectBulletCount = 5;


    private void Awake()
    {
        if (_useTestValue)
        {
            GameManager.Instance.BulletsCountManager.
                BulletCountHome[BulletType.StandardBullet].Value =
                _testStandardBulletCount;

            GameManager.Instance.BulletsCountManager.
                BulletCountHome[BulletType.PenetrateBullet].Value =
                _testPenetrateBulletCount;

            GameManager.Instance.BulletsCountManager.
                BulletCountHome[BulletType.ReflectBullet].Value =
                _testReflectBulletCount;
        }
    }
#endif
}
