using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離攻撃の武器のクラス
/// Enemy_RangeAttackオブジェクトが使用する
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
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
    [Header("攻撃時に再生される音の名前")]
    [SerializeField] private string _attackSEName;

    private Stack<EnemyBullet> _pool;

    protected virtual void Awake()
    {
        if (_poolObject == null)
        {
            CreatePoolObject();
        }

        _pool = new Stack<EnemyBullet>(_poolQuantity);
        CreatePool();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void ReleasePoolObjectInstance() => _poolObject = null;

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

    public void Attack()
    {
        EnemyBullet bullet = PopPool();
        if (bullet == null) return;

        bullet.transform.position = _muzzle.position;
        bullet.SetVelocity(GetBulletDirection());

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", _attackSEName);
    }

    protected virtual Vector3 GetBulletDirection() => _muzzle.right * _muzzle.transform.localScale.x;
}