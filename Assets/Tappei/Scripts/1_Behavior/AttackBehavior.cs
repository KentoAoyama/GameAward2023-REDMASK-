using UnityEngine;
using UniRx;

/// <summary>
/// 攻撃を行う際に使用するクラス
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    void Awake()
    {

    }

    public void Attack()
    {
        Debug.Log("攻撃しました");
    }
}
