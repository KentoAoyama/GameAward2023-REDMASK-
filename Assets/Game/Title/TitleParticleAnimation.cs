using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleParticleAnimation : MonoBehaviour
{
    [SerializeField, Tooltip("Animationのターゲットの座標")]
    private Vector3 _target = new Vector3();

    private Vector3 _originalPosition = new Vector3();
    private void Awake()
    {
        _originalPosition = transform.position;
    }

    public void AnimPlay()
    {
        if (transform.position != _target)
        {
            this.transform.DOMove(_target, 0.5f);
        }
        else
        {
            this.transform.DOMove(_originalPosition, 0.5f);
        }
    }
}
