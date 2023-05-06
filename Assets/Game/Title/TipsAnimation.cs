using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipsAnimation : MonoBehaviour
{
    private Text _tipsText = default;

    private void Awake()
    {
        _tipsText = GetComponent<Text>();
    }

    public void ChangeTips(string text)
    {
        _tipsText.DOText(text, 1.0f, scrambleMode: ScrambleMode.Uppercase).SetEase(Ease.Linear);
    }
}
