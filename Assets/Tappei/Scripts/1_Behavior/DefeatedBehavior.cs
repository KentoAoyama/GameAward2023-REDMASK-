using UnityEngine;

/// <summary>
/// 撃破された
/// </summary>
public class DefeatedBehavior : MonoBehaviour
{
    [Header("撃破されたさいのエフェクト")]
    [SerializeField] GameObject _defeatedEffectPrefab;

    public void Defeated()
    {
        Instantiate(_defeatedEffectPrefab, transform.position, Quaternion.identity);

        gameObject.transform.position = new Vector3(100, 100, 100);
        gameObject.SetActive(false);
    }
}
