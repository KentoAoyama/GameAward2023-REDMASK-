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

    public void DeadSound()
    {
        //音を鳴らす
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Death");
    }

    public void Dead()
    {
        //死亡パネルの非表示
        _deadPanel.SetActive(true);
    }
}
