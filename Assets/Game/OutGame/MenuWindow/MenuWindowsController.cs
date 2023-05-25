using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuWindowsController : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime = 1f;

    private Graphic[] _graphics;

    public void Active()
    {
        if (_graphics == null)
        {
            _graphics = gameObject.GetComponentsInChildren<Graphic>();
            foreach (var graphic in _graphics)
            {
                graphic.DOFade(0f, 0f);
            }
        }
        foreach (var graphic in _graphics) 
        {
            if (graphic.gameObject.name != "Behind obj dont touch image")
            graphic.DOFade(1f, _fadeTime);
        }
    }

    public IEnumerator Inactive()
    {
        foreach (var graphic in _graphics)
        {
            graphic.DOFade(0f, _fadeTime);
        }
        yield return _fadeTime;
        gameObject.SetActive(false);
    }
}
