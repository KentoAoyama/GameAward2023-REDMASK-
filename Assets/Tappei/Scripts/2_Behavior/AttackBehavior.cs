using UnityEngine;

/// <summary>
/// 攻撃を行う際に使用するクラス
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("武器のオブジェクト")]
    [SerializeField] private MonoBehaviour _weapon;

    private IEnemyWeapon _enemyWeapon;
    private IGuidelineDrawer _guidelineDrawer;

    private void Awake()
    {
        if (!_weapon.TryGetComponent(out _enemyWeapon))
        {
            Debug.LogError("IEnemyWeaponを実装したコンポーネントではありません: " + _weapon);
        }

        _weapon.TryGetComponent(out _guidelineDrawer);
    }

    /// <summary>
    /// 武器のクラスにIGuidelineDrawerが実装されている場合は処理が実行される
    /// 毎フレーム呼ばれて攻撃の予告線を表示する
    /// </summary>
    public void DrawGuideline()
    {
        _guidelineDrawer?.DrawGuideline();
    }

    public void Attack()
    {
        // TODO:攻撃の判定が出るまでディレイが欲しい

        _enemyWeapon.Attack();
    }
}
