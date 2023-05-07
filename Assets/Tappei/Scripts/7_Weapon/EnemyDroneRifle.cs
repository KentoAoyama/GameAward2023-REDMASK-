using UnityEngine;

/// <summary>
/// �h���[�����g�p���镐��̃N���X
/// </summary>
public class EnemyDroneRifle : EnemyRifle
{
    [Header("�V�[����ɔz�u����Ă���v���C���[�̃^�O")]
    [SerializeField, TagName] private string _playerTagName;
    [Header("�G�{�̂̎��E�Ɠ����l���g�p���邩")]
    [Tooltip("�e�I�u�W�F�N�g��EnemyController���A�^�b�`����Ă���ꍇ�ɗL��")]
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
            Debug.LogWarning("EnemyController���擾�ł��܂���ł����B�f�t�H���g�̒l���g�p���܂��B");
        }
    }
}
