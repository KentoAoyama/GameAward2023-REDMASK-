using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _slowTimePanel;


    public void PanelActive(bool a)
    {
        _slowTimePanel.SetActive(a);
    }
    
}
