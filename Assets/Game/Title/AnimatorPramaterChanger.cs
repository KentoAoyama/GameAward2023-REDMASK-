using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnimatorPramaterChanger : MonoBehaviour
{
    [SerializeField]
    private Animator _anim = default;
    [SerializeField, Tooltip("変更したいパラメータの名前")]
    private string _paramaterName;
    [SerializeField, Tooltip("パラメータにセットしたい値")]
    private bool _paramaterValue;
    private Button _button = default;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _anim.SetBool(_paramaterName, _paramaterValue));
    }
}
