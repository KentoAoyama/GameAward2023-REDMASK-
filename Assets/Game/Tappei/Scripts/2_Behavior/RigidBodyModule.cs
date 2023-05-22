using UnityEngine;

/// <summary>
/// Rigidbodyを操作するクラス
/// MoveBehaviorクラスから使用される
/// </summary>
[System.Serializable]
public class RigidBodyModule
{
    /// <summary>
    /// 移動先に到着した際にぷるぷるしないようにする為の値
    /// 値を大きくすればより正確に移動先にたどり着くが、速度次第ではぷるぷるしてしまう
    /// </summary>
    private static readonly float ArrivalTolerance = 500.0f;

    [SerializeField] private Rigidbody2D _rigidbody;
    
    /// <summary>
    /// ポーズしたときにVelocityを一旦保存しておくための変数
    /// </summary>
    private Vector3 _tempVelocity;

    /// <summary>
    /// Velocityをターゲットの方向に向けることでターゲットへの移動を行う
    /// スローモーションとポーズに対応している
    /// </summary>
    public void SetVelocityToTarget(Vector3 targetPos, float moveSpeed, Transform transform)
    {
        Vector3 velo = targetPos - transform.position;

        bool isArrival = velo.sqrMagnitude < moveSpeed / ArrivalTolerance;
        velo = isArrival ? Vector3.zero : Vector3.Normalize(velo) * moveSpeed;
        velo.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velo * GameManager.Instance.TimeController.EnemyTime;
    }

    /// <summary>
    /// 自然落下させる為のVelocityを設定する
    /// 移動をキャンセルした場合に呼ばれる
    /// </summary>
    public void SetFallVelocity()
    {
        Vector3 velo = _rigidbody.velocity;
        velo.x = 0;
        velo.z = 0;
        _rigidbody.velocity = velo;
    }

    /// <summary>
    /// 坂道で静止している際の滑り止めを行う処理
    /// 移動開始時にもisKinematicの有効化のために呼ばれる
    /// </summary>
    public void UpdateKinematic(bool isKinematic)
    {
        if (isKinematic)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            SetFallVelocity();
        }

        _rigidbody.isKinematic = isKinematic;
    }

    /// <summary>
    /// ポーズ時に速度を一時的に保存する
    /// </summary>
    public void SaveVelocity()
    {
        _rigidbody.isKinematic = true;
        _tempVelocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// ポーズ解除時に一時保存していた速度を割り当てる
    /// </summary>
    public void LoadVelocity()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _tempVelocity;
    }

    /// <summary>
    /// commandで使用可能
    /// </summary>
    public void AddForce()
    {
        _rigidbody.mass = 1;
        _rigidbody.isKinematic = false;
        SetFallVelocity();
        float rx = Random.Range(-20.0f, 20.0f);
        _rigidbody.AddForce(new Vector2(rx, 10.0f), ForceMode2D.Impulse);
    }
}
