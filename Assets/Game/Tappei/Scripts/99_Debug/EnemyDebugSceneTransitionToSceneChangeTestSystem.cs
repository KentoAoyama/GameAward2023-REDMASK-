using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�ڂŃo�O���o�Ȃ����e�X�g���邽�߂̃N���X
/// </summary>
public class EnemyDebugSceneTransitionToSceneChangeTestSystem : MonoBehaviour
{
    public void Execute()
    {
        SceneManager.LoadScene("SceneChangeTest");
    }
}
