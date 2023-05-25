using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Time/Create TimeInformationSetting")]
public class TimeInformation : ScriptableObject
{
    [Header("ŽžŠÔ‚ª’x‚¢‚Æ‚«‚Ì”{—¦")]

    [Header("ƒvƒŒƒCƒ„[‚ÌŽž’x‚Ì”{—¦")]
    [Tooltip("ƒvƒŒƒCƒ„[‚ÌŽž’x‚Ì”{—¦"), SerializeField] private float _playerSlowSpeed = 0.5f;

    [Header("“G‚ÌŽž’x‚Ì”{—¦")]
    [Tooltip("“G‚ÌŽž’x‚Ì”{—¦"), SerializeField] private float _enemySpeed = 0.3f;

    [Header("’e‚ÌŽž’x‚Ì”{—¦")]
    [Tooltip("’e‚ÌŽž’x‚Ì”{—¦"), SerializeField] private float _bulletSpeed = 0.5f;

    [Header("ƒJƒƒ‰‚ÌŽž’x‚Ì”{—¦")]
    [Tooltip("ƒJƒƒ‰‚ÌŽž’x‚Ì”{—¦"), SerializeField] private float _cameraSpeed = 0.5f;

    //[Header("ƒqƒbƒgƒXƒgƒbƒv‚ÌŽžŠÔ")]
    //[Tooltip("ƒqƒbƒgƒXƒgƒbƒv‚ÌŽžŠÔ"), SerializeField] private float _hitStopTime = 0.5f;

    public float PlayerSlowSpeed => _playerSlowSpeed;

    public float EnemySlowSpeed => _enemySpeed;

    public float BulletSlowSpeed => _bulletSpeed;

    public float CameraSpeed => _cameraSpeed;

    //public float HitStopTime => _hitStopTime;

}
