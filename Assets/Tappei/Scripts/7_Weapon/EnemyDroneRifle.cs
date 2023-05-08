using UnityEngine;

/// <summary>
/// ドローンが使用する武器のクラス
/// </summary>
public class EnemyDroneRifle : EnemyRifle
{
    [Header("シーン上に配置されているプレイヤーのタグ")]
    [SerializeField, TagName] private string _playerTagName;
    [Header("敵本体の視界と同じ値を使用するか")]
    [Tooltip("親オブジェクトにEnemyControllerがアタッチされている場合に有効")]
    [SerializeField] private bool _isLinkEnemyParams = true;

    private Transform _player;
    private SightSensor _sightSensor;
    private float _sightRadius = 10;
    private float _maxAngle = 360;

    protected override void Awake()
    {
        _sightSensor = GetComponent<SightSensor>();

        if (_isLinkEnemyParams)
        {
            InitParams();
        }

        base.Awake();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
    }

    private void Update()
    {
        SightResult result = _sightSensor.LookForPlayerInSight(_sightRadius,_maxAngle,_sightRadius);
        if (result == SightResult.OutSight) return;

        TurnToPlayer();
    }

    void TurnToPlayer()
    {
        Vector3 dir = _player.transform.position - transform.position;
        transform.right = dir * transform.localScale.x;
    }

    protected override Vector3 GetBulletDirection()
    {
        return (_player.transform.position - _muzzle.position).normalized;
    }

    private void InitParams()
    {
        EnemyController enemyController = GetComponentInParent<EnemyController>();
        if (enemyController != null)
        {
            _sightRadius = enemyController.Params.SightRadius;
            _maxAngle = enemyController.Params.SightAngle;
        }
        else
        {
            Debug.LogWarning("EnemyControllerが取得できませんでした。デフォルトの値を使用します。");
        }
    }
}
