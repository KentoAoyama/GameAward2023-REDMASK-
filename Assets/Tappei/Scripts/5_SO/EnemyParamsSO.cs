using UnityEngine;

/// <summary>
/// ’Êí‚Ì“G‚ÌŠeƒpƒ‰ƒ[ƒ^‚ğİ’è‚·‚éScriptableObject
/// EnemyParamsManager‚É‚½‚¹A“G–ˆ‚ÉQÆ‚·‚é
/// </summary>
[CreateAssetMenu(fileName = "EnemyParams_")]
public class EnemyParamsSO : ScriptableObject
{
    enum State
    {
        Idle,
        Search,
    }

    [Header("ˆÚ“®‘¬“x‚Ìİ’è")]
    [Tooltip("•à‚¢‚ÄˆÚ“®‚·‚éÛ‚Ì‘¬“x")]
    [SerializeField] private float _walkSpeed = 2.0f;
    [Tooltip("‘–‚Á‚ÄˆÚ“®‚·‚éÛ‚Ì‘¬“x")]
    [SerializeField] private float _runSpeed = 4.0f;

    [Header("Searchó‘Ô‚ÌÜ‚è•Ô‚µ’n“_‚Ìİ’è")]
    [Tooltip("Ü‚è•Ô‚·‚Ü‚Å‚Ì‹——£")]
    [SerializeField] private float _turningPoint = 3.0f;
    [Tooltip("Ü‚è•Ô‚µ’n“_‚É•t‚­‘O‚Éƒ‰ƒ“ƒ_ƒ€‚ÉÜ‚è•Ô‚·")]
    [SerializeField] private bool _useRandomTurningPoint;

    [Header("‹ŠE‚Ìİ’è")]
    [Tooltip("îó‚Ì‹ŠE‚Ì”¼Œa")]
    [SerializeField] private float _sightRadius = 9.0f;
    [Tooltip("îó‚Ì‹ŠE‚ÌŠp“x")]
    [SerializeField] private float _sightAngle = 270.0f;
    [Tooltip("ŠÔ‚ÉáŠQ•¨‚ª‚ ‚Á‚½ê‡‚É–³‹‚·‚é")]
    [SerializeField] private bool _isIgnoreObstacle;

    [Header("UŒ‚”ÍˆÍ‚Ìİ’è")]
    [Tooltip("UŒ‚‰Â”\‚È”ÍˆÍ")]
    [SerializeField] private float _attackRange = 3.0f;
    [Tooltip("UŒ‚‚ÌŠÔŠu(•b)")]
    [SerializeField] private float _attackRate = 2.0f;

    [Header("Entry‚Ìó‘Ô")]
    [SerializeField] private State _entryState;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float TurningPoint => _turningPoint;
    public bool UseRandomTurningPoint => _useRandomTurningPoint;
    public float SightRadius => _sightRadius;
    public float SightAngle => _sightAngle;
    public bool IsIgnoreObstacle => _isIgnoreObstacle;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;
    public StateType EntryState
    {
        get
        {
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
}
