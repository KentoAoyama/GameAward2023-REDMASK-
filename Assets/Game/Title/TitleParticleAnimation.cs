using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleParticleAnimation : MonoBehaviour
{
    [SerializeField, Tooltip("Animationのターゲットの座標")]
    private Vector3 _target = new Vector3();

    private Vector3 _originalPosition = new Vector3();
    private bool _isMoving = false;
    private void Awake()
    {
        _originalPosition = transform.position;
    }

    public void AnimPlay()
    {
        if (_isMoving) return;

        if (transform.position != _target)
        {
            _isMoving = true;
            this.transform.DOMove(_target, 0.45F).
                OnComplete(() => _isMoving = false);
        }
        else
        {
            _isMoving = true;
            this.transform.DOMove(_originalPosition, 0.5f)
                .OnComplete(() => _isMoving = false);
        }
    }
}
