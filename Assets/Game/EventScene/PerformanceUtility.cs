using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceUtility : MonoBehaviour
{
    [Header("���o�Ŏg�p����ϐ�")]
    [SerializeField]
    private CinemachineImpulseSource _impulse;
    [SerializeField]
    private LineRenderer _line;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _fadePanel;

    [Header("�J�����̐U���֌W")]
    [SerializeField]
    private Transform _cameraPos;
    [SerializeField]
    private float _shakePower = 0.1f;
    [SerializeField]
    private int _shakeNumber = 50;

    private void Start()
    {
        _line.enabled = false;
        _fadePanel.gameObject.SetActive(true);
    }

    public void FadeIn(float duration)
    {
        _fadePanel.DOFade(0f, duration);
    }

    public void FadeOut(float duration)
    {
        _fadePanel.DOFade(1f, duration);
    }

    public void EnemyBrokenSEPlay()
    {
        Debug.Log("SE�Đ�");
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
    }

    public void GunShoot(float interval)
    {
        Debug.Log("�ˌ�");
        Impulse(0.2f);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Gun");

        StartCoroutine(Shoot(interval));
    }

    private IEnumerator Shoot(float interval)
    {
        if (_line == null) yield break;
        _line.enabled = true;
        yield return new WaitForSeconds(interval);
        _line.enabled = false;
    }

    public void Impulse(float shakeTime)
    {
        _cameraPos.DOShakePosition(shakeTime, _shakePower, _shakeNumber, 90, false);
    }

    public void FadeInText(float duration)
    {
        _text.DOFade(1f, duration);
    }

    public void FadeOutText(float duration)
    {
        _text.DOFade(0f, duration);
    }

    public void BGMPlay(string BGMName)
    {
        GameManager.Instance.AudioManager.PlayBGM("CueSheet_Gun", BGMName);
    }

    public void TrueEnding()
    {
        Debug.Log("TrueEnding!!");
    }
}
