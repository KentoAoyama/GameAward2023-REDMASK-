using UnityEngine;

/// <summary>
/// �G�̊e�v�f��i�s�����ɉ����č��E���]������N���X
/// MoveBehavior�N���X����g�p�����
/// </summary>
[System.Serializable]
public class TurnModule
{
    [SerializeField] private Transform _sprite;
    [SerializeField] private Transform _eye;
    [SerializeField] private Transform _weapon;

    /// <summary>
    /// �^�[�Q�b�g�̈ʒu�ɉ����ĉ�]������
    /// </summary>
    public void TurnTowardsTarget(Vector3 targetPos, Transform transform)
    {
        int dir = GetDirectionTowardsTarget(targetPos, transform);
        UpdateSpriteDirection(dir, _sprite);
        UpdateEyePositionAndDirection(dir, _eye);
        UpdateWeaponPositionAndDirection(dir, _weapon);
    }

    private int GetDirectionTowardsTarget(Vector3 targetPos, Transform transform)
    {
        float diff = targetPos.x - transform.position.x;
        return (int)Mathf.Sign(diff);
    }

    /// <summary>
    /// �ڕW�̈ʒu�ɉ����ăL�����N�^�[��Sprite�����E���]������
    /// </summary>
    private void UpdateSpriteDirection(int dir, Transform sprite)
    {
        Vector3 scale = new Vector3(dir * Mathf.Abs(sprite.localScale.x), sprite.localScale.y, 1);
        sprite.localScale = scale;
    }

    /// <summary>
    /// ���E�̌��_�����E�ɉ������ʒu�Ɉړ������E���]������
    /// </summary>
    private void UpdateEyePositionAndDirection(int dir, Transform eye)
    {
        Vector3 eyePos = eye.localPosition;
        eyePos.x = Mathf.Abs(eyePos.x) * dir;
        eye.localPosition = eyePos;

        eye.eulerAngles = Vector3.forward * (dir == 1 ? 0 : 180);
    }

    /// <summary>
    /// ����̈ʒu�����E�ɉ������ʒu�Ɉړ������E���]������
    /// </summary>
    private void UpdateWeaponPositionAndDirection(int dir, Transform weapon)
    {
        Vector3 weaponPos = weapon.localPosition;
        weaponPos.x = Mathf.Abs(weaponPos.x) * dir;
        weapon.localPosition = weaponPos;

        weapon.localScale = new Vector3(dir, 1, 1);
    }
}
