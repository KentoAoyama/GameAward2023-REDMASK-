using UnityEngine;

/// <summary>
/// プレイヤーが撃って起爆するドラム缶のクラス
/// </summary>
public class DrumCan : MonoBehaviour, IPausable, IDamageable
{
    [Header("爆発の半径")]
    [SerializeField] float _radius;
    [Header("レイヤーの設定")]
    [Tooltip("プレイヤーにもヒットさせたい場合はプレイヤーのレイヤーも追加すること")]
    [SerializeField] LayerMask _hitLayerMask;
    [Tooltip("障害物となるオブジェクトのレイヤーを割りあてる")]
    [SerializeField] LayerMask _obstacleLayerMask;

    private Animator _animator;
    /// <summary>
    /// ギズモにも表示させるのでメンバとして保持しておく
    /// </summary>
    private Vector3 _rayOrigin;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rayOrigin = transform.position;
    }

    public void Pause()
    {
        // TODO:あれば追加する
    }

    public void Resume()
    {
        // TODO:あれば追加する
    }

    public void Damage()
    {
        // TODO:アニメーション再生後、消える(ポーズとときおそに対応させる)
        gameObject.SetActive(false);
        // TOOD:音の再生

        Collider2D[] results = Physics2D.OverlapCircleAll(_rayOrigin, _radius, _hitLayerMask);
        if (results.Length == 0) return;

        // 範囲内にいる対象に向けてRayを飛ばして障害物にヒットしなければ撃破する
        foreach(Collider2D collider in results)
        {
            Vector3 targetPos = collider.transform.position;

            Vector3 dir = Vector3.Normalize(targetPos - _rayOrigin);
            // 足元が原点なので上方向にオフセットを用意する
            float offsetY = 0.5f;
            dir.y += offsetY;
            float radius = Vector3.Distance(_rayOrigin, targetPos);

#if UNITY_EDITOR
            Debug.DrawRay(_rayOrigin, dir * radius, Color.green, 3.0f);
#endif
            RaycastHit2D hit = Physics2D.Raycast(_rayOrigin, dir, radius, _obstacleLayerMask);

            if (hit) continue;

            collider.GetComponent<IDamageable>().Damage();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
