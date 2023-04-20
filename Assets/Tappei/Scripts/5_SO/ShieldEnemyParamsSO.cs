using UnityEngine;

/// <summary>
/// 盾持ちの敵の各パラメータを設定するScriptableObject
/// EnemyParamsManagerに持たせ、敵毎に参照する
/// </summary>
[CreateAssetMenu(fileName = "ShieldEnemyParams_")]
public class ShieldEnemyParamsSO : EnemyParamsSO
{
    public override StateType EntryState
    {
        get
        {
            // フラグが立っている場合はSeach状態にせず、常にIdle状態となる
            if (_isIdleUndiscovered) return StateType.IdleExtend;

            if (_entryState == State.Idle)
            {
                return StateType.IdleExtend;
            }
            else
            {
                return StateType.SearchExtend;
            }
        }
    }

    [Header("攻撃された場合の硬直時間(秒)の設定")]
    [SerializeField] private float _stiffeningTime = 0.5f;

    public float StiffeningTime => _stiffeningTime;
}
