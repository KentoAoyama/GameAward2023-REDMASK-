using UnityEngine;

/// <summary>
/// �e�X�g�p�̒e
/// </summary>
public class EnemyTestBullet : MonoBehaviour
{
    float _dir = 1;

    public void Init(float dir)
    {
        _dir = dir;
    }

    private void Update()
    {
        transform.Translate(new Vector3(_dir, 0, 0) * 2);
    }
}
