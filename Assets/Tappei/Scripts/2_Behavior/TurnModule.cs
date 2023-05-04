using UnityEngine;

/// <summary>
/// 敵の各要素を進行方向に応じて左右反転させるクラス
/// MoveBehaviorクラスが持つ
/// </summary>
public class TurnModule
{
    private Transform _transform;
    private Transform _sprite;
    private Transform _eye;
    private Transform _weapon;

    public TurnModule(Transform transform, Transform sprite, Transform eye, Transform weapon)
    {
        _transform = transform;
        _sprite = sprite;
        _eye = eye;
        _weapon = weapon;
    }

    /// <summary>
    /// ターゲットの位置に応じて回転させる
    /// </summary>
    public void TurnTowardsTarget(Vector3 targetPos)
    {
        int dir = GetDirectionTowardsTarget(targetPos, _transform);
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
    /// 目標の位置に応じてキャラクターのSpriteを左右反転させる
    /// </summary>
    private void UpdateSpriteDirection(int dir, Transform sprite)
    {
        Vector3 scale = new Vector3(dir * Mathf.Abs(sprite.localScale.x), sprite.localScale.y, 1);
        sprite.localScale = scale;
    }

    /// <summary>
    /// 視界の原点を左右に応じた位置に移動＆左右反転させる
    /// </summary>
    private void UpdateEyePositionAndDirection(int dir, Transform eye)
    {
        Vector3 eyePos = eye.localPosition;
        eyePos.x = Mathf.Abs(eyePos.x) * dir;
        eye.localPosition = eyePos;

        eye.eulerAngles = Vector3.forward * (dir == 1 ? 0 : 180);
    }

    /// <summary>
    /// 武器の位置を左右に応じた位置に移動＆左右反転させる
    /// </summary>
    private void UpdateWeaponPositionAndDirection(int dir, Transform weapon)
    {
        Vector3 weaponPos = weapon.localPosition;
        weaponPos.x = Mathf.Abs(weaponPos.x) * dir;
        weapon.localPosition = weaponPos;

        weapon.localScale = new Vector3(dir, 1, 1);
    }
}
