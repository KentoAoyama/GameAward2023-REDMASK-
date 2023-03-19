using UnityEngine;

/// <summary>
/// 盾持ちの敵の各パラメータを設定するScriptableObject
/// EnemyParamsManagerに持たせ、敵毎に参照する
/// </summary>
[CreateAssetMenu(fileName = "ShieldEnemyParams_")]
public class ShieldEnemyParamsSO : EnemyParamsSO
{
    [Header("攻撃された場合の硬直時間(秒)の設定")]
    [SerializeField] private float _stiffeningTime = 0.5f;

    public float StiffeningTime => _stiffeningTime;
}
