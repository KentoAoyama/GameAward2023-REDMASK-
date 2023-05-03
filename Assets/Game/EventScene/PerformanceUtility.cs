using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceUtility : MonoBehaviour
{
    [Header("ââèoÇ≈égópÇ∑ÇÈïœêî")]
    [SerializeField]
    private CinemachineImpulseSource _impulse;
    [SerializeField]
    private LineRenderer _line;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _fadePanel;

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
        Debug.Log("SEçƒê∂");
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
    }

    public void GunShoot(float interval)
    {
        Debug.Log("éÀåÇ");
        Impulse();
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

    public void Impulse()
    {
        _impulse?.GenerateImpulse();
    }

    public void FadeInText(float duration)
    {
        _text.DOFade(1f, duration);
    }

    public void FadeOutText(float duration)
    {
        _text.DOFade(0f, duration);
    }
}
