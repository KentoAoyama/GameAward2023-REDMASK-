using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CylinderContinueGrayOut : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();

        if (GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber() <= 0)
        {
            Debug.Log("A");
            _text.color = Color.gray;
            _button.enabled = false;
        } 
    }
}
