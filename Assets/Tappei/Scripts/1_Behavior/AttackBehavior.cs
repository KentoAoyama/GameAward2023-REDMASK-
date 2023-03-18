using UnityEngine;

/// <summary>
/// 攻撃を行う際に使用するクラス
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("弾のプレハブ")]
    [SerializeField] GameObject _bulletPreafb;
    [Header("弾を発射する位置")]
    [SerializeField] Transform _muzzle;

    public void Attack()
    {
        GameObject instance = Instantiate(_bulletPreafb, _muzzle.position, Quaternion.identity);
        instance.GetComponent<EnemyTestBullet>().Init(_muzzle.localScale.x);
    }
}
