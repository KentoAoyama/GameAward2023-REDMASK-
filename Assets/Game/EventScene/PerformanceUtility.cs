using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceUtility : MonoBehaviour
{
    [Header("演出で使用する変数")]
    [SerializeField]
    private CinemachineImpulseSource _impulse;
    [SerializeField]
    private LineRenderer _line;

    private void Start()
    {
        _line.enabled = false;
    }

    public void EnemyBrokenSEPlay()
    {
        Debug.Log("SE再生");
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
    }

    public void GunShoot()
    {
        Debug.Log("射撃");
        Impulse();
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Gun");
        if (_line != null)
        _line.enabled = true;

    }

    private void Impulse()
    {
        _impulse?.GenerateImpulse();
    }
}
