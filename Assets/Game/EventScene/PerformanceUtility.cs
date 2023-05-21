using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PerformanceUtility : MonoBehaviour
{
    [Header("演出で使用する変数")]
    [SerializeField]
    private CinemachineImpulseSource _impulse;
    [SerializeField]
    private LineRenderer _line;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _fadePanel;

    [Header("カメラの振動関係")]
    [SerializeField]
    private Transform _cameraPos;
    [SerializeField]
    private float _shakePower = 0.1f;
    [SerializeField]
    private int _shakeNumber = 50;

    [Header("移行するシーン")]
    [SerializeField, SceneName]
    private string _sceneName;

    int _seIndex = -1;

    private void Start()
    {
        if (_line != null)
        _line.enabled = false;

        if (_fadePanel != null)
        _fadePanel.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            TrueEnding();
        }
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
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
    }

    public void BackGroundInsidePlay()
    {
        _seIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_BackGround_Outside");
    }

    public void BackGroundOutsidePlay()
    {
        _seIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_BackGround_Outside");
    }

    public void BackGroundStop()
    {
        GameManager.Instance.AudioManager.StopSE(_seIndex);
    }

    public void GunShoot(float interval)
    {
        Debug.Log("射撃");
        Impulse(0.2f);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Gun");

        StartCoroutine(Shoot(interval));
    }

    public void GunShootSEPlay()
    {
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Gun");
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
        SceneManager.LoadScene(_sceneName);
    }
}
