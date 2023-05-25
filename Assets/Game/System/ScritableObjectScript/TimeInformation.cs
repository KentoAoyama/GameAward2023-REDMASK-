using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time/Create TimeInformationSetting")]
public class TimeInformation : ScriptableObject
{
    [Header("���Ԃ��x���Ƃ��̔{��")]

    [Header("�v���C���[�̎��x�̔{��")]
    [Tooltip("�v���C���[�̎��x�̔{��"), SerializeField] private float _playerSlowSpeed = 0.5f;

    [Header("�G�̎��x�̔{��")]
    [Tooltip("�G�̎��x�̔{��"), SerializeField] private float _enemySpeed = 0.3f;

    [Header("�e�̎��x�̔{��")]
    [Tooltip("�e�̎��x�̔{��"), SerializeField] private float _bulletSpeed = 0.5f;

    [Header("�J�����̎��x�̔{��")]
    [Tooltip("�J�����̎��x�̔{��"), SerializeField] private float _cameraSpeed = 0.5f;

    //[Header("�q�b�g�X�g�b�v�̎���")]
    //[Tooltip("�q�b�g�X�g�b�v�̎���"), SerializeField] private float _hitStopTime = 0.5f;

    public float PlayerSlowSpeed => _playerSlowSpeed;

    public float EnemySlowSpeed => _enemySpeed;

    public float BulletSlowSpeed => _bulletSpeed;

    public float CameraSpeed => _cameraSpeed;

    //public float HitStopTime => _hitStopTime;

}
