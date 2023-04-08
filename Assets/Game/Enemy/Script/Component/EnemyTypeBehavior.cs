// 日本語対応
using UnityEngine;

public class EnemyTypeBehavior : MonoBehaviour
{
    [SerializeField]
    private EnemyType _enemyType = default;

    public EnemyType EnemyType => _enemyType;

    private void OnEnable()
    {
        GameManager.Instance.EnemyRegister.Register(this);
    }
    private void OnDisable()
    {
        GameManager.Instance.EnemyRegister.Lift(this);
    }
}
