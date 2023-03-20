using UnityEngine;

/// <summary>
/// 攻撃を行う際に使用するクラス
/// 各ステートからBehaviorFacadeクラス経由で呼び出される
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("弾のプレハブ")]
    [SerializeField] private GameObject _bulletPreafb;
    [Header("弾を発射する位置")]
    [SerializeField] private Transform _muzzle;

    public void Attack()
    {
        Debug.Log("攻撃");
    }
}
