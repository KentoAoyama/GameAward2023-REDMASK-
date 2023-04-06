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

    [Tooltip("Discover状態のAnimationClipを割り当てる")]
    [SerializeField] AnimationClip _discoverAnimClip;
    [Tooltip("Dead状態のAnimationClipを割り当てる")]
    [SerializeField] AnimationClip _deadAnimClip;

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

    [Header("Entry時の状態")]
    [SerializeField] protected State _entryState;
    [Header("プレイヤー未発見時は常にSearch状態にする")]
    [SerializeField] protected bool _isAlwaysSearching;

    public float DiscoverStateTransitionDelay => _discoverAnimClip.length;
    public float DefeatedStateTransitionDelay => _deadAnimClip.length;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float TurningPoint => _turningPoint;
    public bool UseRandomTurningPoint => _useRandomTurningPoint;
    public float SightRadius => _sightRadius;
    public float SightAngle => _sightAngle;
    public bool IsIgnoreObstacle => _isIgnoreObstacle;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;
    public bool IsAlwaysSearching => _isAlwaysSearching;
    public virtual StateType EntryState
    {
        get
        {
            // フラグが立っている場合はIdle状態にせず、常にSearch状態となる
            if (_isAlwaysSearching) return StateType.Search;

            if (_entryState == State.Idle)
            {
                return StateType.Idle;
            }
            else
            {
                return StateType.Search;
            }
        }
    }

    // ここから下はプランナーに弄らせない値
    // ただし、要望があった際にはインスペクターで割り当てられるように変更可能
    public float MinTransitionTimeElapsed => 1.0f;
    public float MaxTransitionTimeElapsed => 2.0f;
    public float MoveCancelTimerThreshold => 0.25f;

    public int GetAnimationHash(AnimationName name) => Animator.StringToHash(name.ToString());
}
