using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTestMove : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        Debug.Log(GameManager.Instance.TimeController.EnemyTime);
        _animator.speed = GameManager.Instance.TimeController.EnemyTime;
    }
}
