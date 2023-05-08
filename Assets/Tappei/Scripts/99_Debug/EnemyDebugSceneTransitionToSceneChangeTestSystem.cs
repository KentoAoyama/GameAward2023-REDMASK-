using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移でバグが出ないかテストするためのクラス
/// </summary>
public class EnemyDebugSceneTransitionToSceneChangeTestSystem : MonoBehaviour
{
    public void Execute()
    {
        SceneManager.LoadScene("SceneChangeTest");
    }
}
