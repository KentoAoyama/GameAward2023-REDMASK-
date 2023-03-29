// 日本語対応
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class PauseCountPresenter : MonoBehaviour
{
    [SerializeField]
    private Text _pauseCountText = default;

    private void Update()
    {
        _pauseCountText.text = $"Pause Count : {GameManager.Instance.PauseManager.PauseCounter}";
    }
}
