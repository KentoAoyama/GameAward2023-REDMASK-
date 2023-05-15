using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離攻撃の武器のクラス
/// Enemy_RangeAttackオブジェクトが使用する
/// </summary>
[RequireComponent(typeof(EnemyWeaponGuidelineDrawer))]
public class EnemyRifle : MonoBehaviour, IEnemyWeapon, IGuidelineDrawer
{
    /// <summary>
    /// 敵毎にこのオブジェクトの子に弾を生成していく
    /// ドローンと遠距離攻撃の敵が使用する弾が同じという想定
    /// </summary>
    private static Transform _poolObject;

    [Header("発射する弾に関する設定")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Tooltip("プールする敵弾の数、攻撃頻度を上げる場合はこちらも上げないといけない")]
    [SerializeField] private int _poolQuantity;
    [Tooltip("弾が発射されるマズル、飛ぶ方向の左右の制御はスケールのxを-1にすることで行う")]
    [SerializeField] protected Transform _muzzle;

    private EnemyWeaponGuidelineDrawer _guidelineDrawer;
    private Stack<EnemyBullet> _pool;

    private void Awake()
    {
        InitOnAwake();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void ReleasePoolObjectInstance() => _poolObject = null;

    protected virtual void InitOnAwake()
    {
        if (_poolObject == null)
        {
            CreatePoolObject();
        }

        _guidelineDrawer = GetComponent<EnemyWeaponGuidelineDrawer>();
        _pool = new Stack<EnemyBullet>(_poolQuantity);
        CreatePool();
    }

    private void CreatePoolObject()
    {
        GameObject poolObject = new GameObject();
        poolObject.name = "EnemyBulletPool";
        _poolObject = poolObject.transform;
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolQuantity; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity, _poolObject);
            bullet.InitSetPool(_pool);
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }

    /// <summary>
    /// プールから取り出す処理
    /// 戻す処理は弾側に持たせている
    /// </summary>
    protected EnemyBullet PopPool()
    {
        if (_pool.Count > 0)
        {
            EnemyBullet bullet = _pool.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            return null;
        }
    }

    public void DrawGuideline()
    {
        _guidelineDrawer.Draw(_muzzle.position, GetBulletDirection());
    }

    public void Attack()
    {
        EnemyBullet bullet = PopPool();
        if (bullet == null) return;

        bullet.transform.position = _muzzle.position;
        bullet.SetVelocity(GetBulletDirection());
    }

    /// <summary>
    /// このメソッドをオーバーライドすることで弾を飛ばす方向を変更できる
    /// </summary>
    protected virtual Vector3 GetBulletDirection() => Vector3.right * _muzzle.transform.localScale.x;
}