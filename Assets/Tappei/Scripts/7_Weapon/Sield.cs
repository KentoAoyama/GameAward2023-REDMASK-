using UnityEngine;

/// <summary>
/// �������̓G���������鏂
/// �e���q�b�g�����Ƃ��ɔ�\���ɂȂ����̂�ShieldEnemyController�������m����
/// </summary>
public class Sield : MonoBehaviour, IDamageable
{
    public void Damage() => gameObject.SetActive(false);
}
