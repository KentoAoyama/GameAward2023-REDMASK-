using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadAnimEvent : MonoBehaviour
{
    [SerializeField] private GameObject _deadPanel;

    private void Awake()
    {
        _deadPanel.SetActive(false);
    }


    public void Dead()
    {
        //���S�p�l���̔�\��
        _deadPanel.SetActive(true);
    }
}
