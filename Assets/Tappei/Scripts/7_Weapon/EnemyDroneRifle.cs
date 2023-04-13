using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _sightSensor = GetComponent<SightSensor>();

        if (_isLinkEnemyParams)
        {
            InitParams();
        }
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTagName).transform;
    }

    private void Update()
    {
        float result = _sightSensor.TryGetDistanceToPlayer(_sightRadius, _maxAngle);
        if (result > SightSensor.PlayerOutSight)
        {
            TurnToPlayer();
        }
    }

    void TurnToPlayer()
    {
        Vector3 dir = _player.transform.position - transform.position;
        transform.right = dir * transform.localScale.x;
    }

    public override void Attack()
    {
        // 攻撃処理を書く
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
