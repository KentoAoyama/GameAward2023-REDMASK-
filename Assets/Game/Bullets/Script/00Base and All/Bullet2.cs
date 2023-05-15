// 日本語対応
using Bullet;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 弾は指定の場所まで高速で移動する。
/// 前のポジションから現在のポジションに向かって LineCastAll() し,
/// IDamageable()を持つオブジェクトに対してDamage()を 指定回数 実行する。
/// 
/// </summary>
public class Bullet2 : MonoBehaviour, IPausable, IStoreableInChamber
{
    [Tooltip("この数値が 0以下であれば,無数の敵にヒットする。\n1以上であれば その数値だけの敵にヒットする。")]
    [SerializeField]
    private int _maxHitCount = 0;
    [SerializeField]
    private float _moveSpeed = 0f;
    [SerializeField]
    BulletType _bulletType = default;
    [SerializeField]
    private float _guidelineLength = 0f;
    [SerializeField]
    private Sprite _cylinderUISprite = default;
    [SerializeField]
    private LayerMask _targetLayer = default;

    private int _currentHitCount = 0;
    private List<Vector2> _targetPositions = null;
    private HashSet<IDamageable> _damaged = new HashSet<IDamageable>();
    private Vector2 _currentTargetPosition = default;
    private bool _isComplete = false;

    private Vector2 _previousPosition = default;
    private Vector2 _moveDir = default;
    /// <summary>
    /// 敵のシールドを貫くかどうかを表現する値
    /// </summary>
    protected virtual bool IsPenetrateShield => true;
    private string _shieldTagName = "Shield";


    public BulletType Type => _bulletType;
    public float GuidelineLength => _guidelineLength;
    public Sprite CylinderUISprite => _cylinderUISprite;

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        // 指定の方向、指定の速度で移動する。
        transform.Translate(_moveDir.normalized * Time.deltaTime * _moveSpeed);

        // 左右の補正（行き過ぎた場合、目標地点に強制移動する。）
        if (_moveDir.x >= 0f && transform.position.x > _currentTargetPosition.x ||
            _moveDir.x <= 0f && transform.position.x < _currentTargetPosition.x)
        {
            var pos = transform.position;
            pos.x = _currentTargetPosition.x;
            transform.position = pos;
        }
        // 上下の補正（行き過ぎた場合、目標地点に強制移動する。）
        if (_moveDir.y >= 0f && transform.position.y > _currentTargetPosition.y ||
            _moveDir.y <= 0f && transform.position.y < _currentTargetPosition.y)
        {
            var pos = transform.position;
            pos.y = _currentTargetPosition.y;
            transform.position = pos;
        }

        // 前フレームの座標から現在フレームの座標の間のコライダーをラインキャストを使用して取得する。
        var hits = Physics2D.LinecastAll(_previousPosition, transform.position, _targetLayer);

        // 見つけたIDamageableに対して可能であればDamage()を実行する。
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out IDamageable damageable))
            {
                // 同じオブジェクトから何度もDamage()を呼ばないための処理
                if (!_damaged.Contains(damageable))
                {
                    _damaged.Add(damageable);
                    damageable.Damage();
                    _currentHitCount++;
                }

                // シールドを貫通しないオブジェクトはシールドに接触した時点で消滅する。
                if (!IsPenetrateShield && hits[i].collider.tag == _shieldTagName)
                {
                    Destroy(this.gameObject);
                    return;
                }

                // 指定回数ヒットしたらこのオブジェクトを破棄する。
                // 最大ヒット数が0以下であれば処理しない。
                if (_maxHitCount <= _currentHitCount && _maxHitCount > 0)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
        // 設定された全ての目的地に到達したとき、このオブジェクトを非アクティブにする。
        if (_isComplete)
        {
            gameObject.SetActive(false);
        }
        _previousPosition = transform.position;
    }
    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Register(this);
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Lift(this);
    }

    /// <summary>
    /// 移動先を指定する
    /// </summary>
    /// <param name="positions"> 目的地のx,y座標 </param>
    public void SetPositions(List<Vector2> positions)
    {
        _targetPositions = positions;
    }
    /// <summary>
    /// 移動速度を指定する
    /// </summary>
    /// <param name="speed"> 移動速度 </param>
    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }
    /// <summary>
    /// 移動開始処理。
    /// 設定された座標に向かって移動を開始する。
    /// </summary>
    public async void StartMove()
    {
        // 終了条件 下記の条件どれか一つでも満たす場合、移動せず処理を終了する。
        if (_targetPositions == null || _targetPositions.Count == 0 || _moveSpeed < 0f)
        {
            this.gameObject.SetActive(false);
            return;
        }
        // 現在地を開始地点とする。
        Vector2 startPos = transform.position;
        for (int i = 0; i < _targetPositions.Count; i++)
        {
            try
            {
                // 現在の目的地を取得する。WaitMove等の関数内で現在の目的地を利用するため。
                _currentTargetPosition = _targetPositions[i];
                // 目的地への方向ベクトルを取得。Update関数内で移動に使用する。
                _moveDir = _targetPositions[i] - startPos;
                // 目的地に到達するまで待機する。
                await WaitMove(transform, startPos, _targetPositions[i]);
                // 目的地を新しい開始地点とする。
                startPos = _targetPositions[i];
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
        _isComplete = true;
        return;
    }

    /// <summary>
    /// 指定された地点まで移動するのを待機する。
    /// </summary>
    /// <param name="origin"> 原点 </param>
    /// <param name="startPos"> 開始地点 </param>
    /// <param name="targetPos"> 目的地の座標 </param>
    private async UniTask WaitMove(Transform origin, Vector2 startPos, Vector2 targetPos)
    {
        if (startPos.x <= targetPos.x && // 右上
            startPos.y <= targetPos.y)
        {
            await UniTask.WaitUntil(() =>
                origin.position.x >= targetPos.x &&
                origin.position.y >= targetPos.y,
                cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
        else if (startPos.x >= targetPos.x && // 左上
                 startPos.y <= targetPos.y)
        {
            await UniTask.WaitUntil(() =>
                origin.position.x <= targetPos.x &&
                origin.position.y >= targetPos.y,
                cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
        else if (startPos.x <= targetPos.x && // 右下
                 startPos.y >= targetPos.y)
        {
            await UniTask.WaitUntil(() =>
                origin.position.x >= targetPos.x &&
                origin.position.y <= targetPos.y,
                cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
        else if (startPos.x >= targetPos.x && // 左下
                 startPos.y >= targetPos.y)
        {
            await UniTask.WaitUntil(() =>
                origin.position.x <= targetPos.x &&
                origin.position.y <= targetPos.y,
                cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
        transform.position = targetPos;
    }

    private float _cachedSpeed;
    public void Pause()
    {
        _cachedSpeed = _moveSpeed;
        _moveSpeed = 0f;
    }

    public void Resume()
    {
        _moveSpeed = _cachedSpeed;
    }
}
