// 日本語対応
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReloadButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.PauseManager.ClearCount();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

    }
}
