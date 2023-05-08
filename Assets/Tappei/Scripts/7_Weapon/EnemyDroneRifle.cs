using UnityEngine;
using UniRx;
using UniRx.Triggers;

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
    /// <summary>
    /// デフォルトの視界の半径
    /// </summary>
    private static float _sightRadius = 10;
    /// <summary>
    /// デフォルトの視界の角度
    /// </summary>
    private static float _maxAngle = 360;

    private void Start()
    {
        InitOnStart();
    }

    protected override void InitOnAwake()
    {
        base.InitOnAwake();

        if (_isLinkEnemyParams) InitParams();

        // プレイヤーの方を向く
        this.UpdateAsObservable().Subscribe(_ =>
        {
            SightSensor sightSensor = GetComponent<SightSensor>();
            SightResult result = sightSensor.LookForPlayerInSight(_sightRadius, _maxAngle, _sightRadius);
            if (result == SightResult.OutSight) return;

            TurnToPlayer();
        });
    }

    private void InitOnStart()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
    }

    private void TurnToPlayer()
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
