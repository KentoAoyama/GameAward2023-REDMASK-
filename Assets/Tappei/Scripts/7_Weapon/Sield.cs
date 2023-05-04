using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �������̓G���������鏂
/// ��x�e���󂯂���C�ӂ̃^�C�~���O�܂Ŗ����������
/// </summary>
public class Sield : MonoBehaviour, IDamageable
{
    private Collider2D _collider;

    /// <summary>
    /// ���Ƀv���C���[�̒e���q�b�g�����Ƃ��̃R�[���o�b�N
    /// </summary>
    public UnityAction OnDamaged;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Damage()
    {
        _collider.enabled = false;
        OnDamaged?.Invoke();
    }

    /// <summary>
    /// �O�����炱�̃��\�b�h���ĂԂ��Ƃŏ����L���������
    /// </summary>
    public void Recover() => _collider.enabled = true;
}
