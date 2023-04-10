using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�肵�������ɂ܂�������ԓG�e�̃N���X
/// EnemyRifle�N���X�Ƀv�[������Ă���A���˂���ۂɃA�N�e�B�u�ɂȂ�
/// </summary>
public class EnemyBullet : MonoBehaviour, IPausable
{
    /// <summary>�r���ŏ����Ĉ�a������悤�������炱�̒l��傫������</summary>
    private static float LifeTime = 3.0f;

    [Header("�q�b�g����^�O�̐ݒ�")]
    [Tooltip("�v���C���[�̃^�O")]
    [SerializeField, TagName] private string PlayerTagName;
    [Tooltip("�ǂȂǂ̃X�e�[�W���̃I�u�W�F�N�g�̃^�O")]
    [SerializeField, TagName] private string WallTagName;
    [Header("�e�̐ݒ�")]
    [SerializeField] private float _speed;

    private Transform _transform;
    private Stack<EnemyBullet> _pool;
    private Vector3 _velocity;
    private float _timer;
    private bool _isPause;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Register(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Lift(this);
    }

    public void Pause() => _isPause = true;
    public void Resume() => _isPause = false;

    /// <summary>�e���������ꂽ�ۂ�EnemyRifle�N���X����Ăяo�����</summary>
    public void InitSetPool(Stack<EnemyBullet> pool) => _pool = pool;
    /// <summary>���˂����ۂ�EnemyRifle�N���X����Ăяo�����</summary>
    public void SetVelocity(Vector3 dir) => _velocity = dir * _speed;

    void Update()
    {
        if (_isPause) return;

        float deltaTime = Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        _timer += deltaTime;
        if (_timer > LifeTime)
        {
            ReturnPool();
        }
        else
        {
            _transform.Translate(_velocity * deltaTime);
        }
    }

    /// <summary>
    /// �v�[���ɖ߂�
    /// ���˂���Ă����莞�Ԍ�A�������̓v���C���[�Ƀq�b�g�����ۂɌĂ΂��
    /// </summary>
    private void ReturnPool()
    {
        _timer = 0;
        gameObject.SetActive(false);
        _pool?.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTagName) &&
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
            ReturnPool();
        }
        else if (collision.CompareTag(WallTagName))
        {
            ReturnPool();
        }
    }
}
