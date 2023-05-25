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
        //‰¹‚ð–Â‚ç‚·
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Death");
    }

    public void Dead()
    {
        //Ž€–Sƒpƒlƒ‹‚Ì”ñ•\Ž¦
        _deadPanel.SetActive(true);
    }
}
