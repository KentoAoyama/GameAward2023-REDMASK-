// 日本語対応

using Bullet;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 弾の数を覚えておくクラス
/// </summary>
public class BulletsCountManager
{
    /// <summary>
    /// アジトにある弾の数
    /// </summary>
    private Dictionary<BulletType, int> _bulletCountHome = new Dictionary<BulletType, int>() {
            { BulletType.StandardBullet, 0 },
            { BulletType.PenetrateBullet, 0 },
            { BulletType.ReflectBullet, 0 } };
    /// <summary>
    /// ステージ内で所持している弾の数
    /// </summary>
    private Dictionary<BulletType, int> _bulletCountStage = new Dictionary<BulletType, int>() {
            { BulletType.StandardBullet, 0 },
            { BulletType.PenetrateBullet, 0 },
            { BulletType.ReflectBullet, 0 } };
    /// <summary>
    /// シリンダーの状態を表現する値
    /// </summary>
    private BulletType[] _cylinder = null;

    public Dictionary<BulletType, int> BulletCountHome => _bulletCountHome;
    public Dictionary<BulletType, int> BulletCountStage => _bulletCountStage;
    public BulletType[] Cylinder
    {
        get => _cylinder; set => _cylinder = value;
    }
}