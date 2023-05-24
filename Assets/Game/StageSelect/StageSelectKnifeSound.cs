// 日本語対応
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectKnifeSound : MonoBehaviour
{
    [SerializeField]
    private float _delayTime = 0.2f;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private async void OnClick()
    {
        await UniTask.Delay((int)(_delayTime * 1000f));
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Knife");
    }
}