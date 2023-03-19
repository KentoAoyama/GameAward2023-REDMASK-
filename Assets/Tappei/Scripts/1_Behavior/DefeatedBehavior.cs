using UnityEngine;

/// <summary>
/// ���j���ꂽ
/// </summary>
public class DefeatedBehavior : MonoBehaviour
{
    [Header("���j���ꂽ�����̃G�t�F�N�g")]
    [SerializeField] GameObject _defeatedEffectPrefab;

    public void Defeated()
    {
        Instantiate(_defeatedEffectPrefab, transform.position, Quaternion.identity);

        gameObject.transform.position = new Vector3(100, 100, 100);
        gameObject.SetActive(false);
    }
}
