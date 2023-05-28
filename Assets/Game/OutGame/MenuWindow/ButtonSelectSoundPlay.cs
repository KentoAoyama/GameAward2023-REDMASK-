using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using Cysharp.Threading.Tasks.Triggers;

public class ButtonSelectSoundPlay : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current
            .ObserveEveryValueChanged(_ => EventSystem.current.currentSelectedGameObject)
            .Subscribe(_ => PlaySE());
    }

    private void PlaySE()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Selection");
        }
    }
}
