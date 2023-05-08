using UnityEngine;

/// <summary>
/// 通常の敵の各パラメータを設定するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "EnemyParams_")]
public class EnemyParamsSO : ScriptableObject
{
    protected enum State
    {
        Idle,
        Search,
    }

    /// <summary>
    /// 死亡した際に死体が消えるまでの追加時間
    /// 死亡のアニメーションに追加で待つ
    /// </summary>
    static readonly float DefeatedStateTransitionDelayAdd = 3.0f;
    /// <summary>
    /// 移動量が0の状態が続いた際にIdle状態に遷移させるまでの時間
    /// Move状態でしか使われないが、State内に設定する値を持たせたくないのでSO内に持つ
    /// </summary>
    public static readonly float MoveCancelTimeThreshold = 0.25f;

    [Header("この項目はプランナーが弄る必要なし")]
    [SerializeField] private AnimationClip _discoverAnimClip;
    [SerializeField] private AnimationClip _deadAnimClip;
    [SerializeField] private string _walkSEName;
    [SerializeField] private string _runSEName;

    [Header("移動速度の設定")]
    [Tooltip("歩いて移動する際の速度")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [Tooltip("走って移動する際の速度")]
    [SerializeField] private float _runSpeed = 4.0f;

    [Header("Search状態の折り返し地点の設定")]
    [Tooltip("折り返すまでの距離")]
    [SerializeField] private float _turningPoint = 3.0f;
    [Tooltip("折り返し地点に付く前にランダムに折り返す")]
    [SerializeField] private bool _useRandomTurningPoint;

    [Header("視界の設定")]
    [Tooltip("扇状の視界の半径")]
    [SerializeField] private float _sightRadius = 9.0f;
    [Tooltip("扇状の視界の角度")]
    [SerializeField] private float _sightAngle = 270.0f;
    [Tooltip("間に障害物があった場合に無視する")]
    [SerializeField] private bool _isIgnoreObstacle;

    [Header("攻撃範囲の設定")]
    [Tooltip("攻撃可能な範囲")]
    [SerializeField] private float _attackRange = 3.0f;
    [Tooltip("攻撃の間隔(秒)")]
    [SerializeField] private float _attackRate = 2.0f;

    [Header("IdleからSearchに状態が遷移するまでの時間")]
    [SerializeField] private float _minIdleStateTimer = 1.0f;
    [SerializeField] private float _maxIdleStateTimer = 2.0f;

    public float DiscoverStateTransitionDelay => _discoverAnimClip != null ? _discoverAnimClip.length : 0;
    public float DefeatedStateTransitionDelay
    {
        get => _deadAnimClip != null ? _deadAnimClip.length + DefeatedStateTransitionDelayAdd : 0;
    }
    public string WalkSEName => _walkSEName;
    public string RunSEName => _runSEName;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float TurningPoint => _turningPoint;
    public bool UseRandomTurningPoint => _useRandomTurningPoint;
    public float SightRadius => _sightRadius;
    public float SightAngle => _sightAngle;
    public bool IsIgnoreObstacle => _isIgnoreObstacle;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;

    public int GetAnimationHash(AnimationName name) => Animator.StringToHash(name.ToString());
    public float GetRandomIdleStateTimer() => Random.Range(_minIdleStateTimer, _maxIdleStateTimer);
}