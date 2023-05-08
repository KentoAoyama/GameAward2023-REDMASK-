using UnityEngine;

/// <summary>
/// ターゲットではなく移動先を指定して移動する場合の移動先を制御するクラス
/// MoveBehaviorクラスから使用される
/// </summary>
[System.Serializable]
public class WaypointModule
{
    private GameObject _searchWaypoint;
    private GameObject _forwardWaypoint;
    /// <summary>
    /// この座標を基準にしてSearch状態の移動を行う
    /// </summary>
    private Vector3 _footPos;

    /// <summary>
    /// 前回の左右の移動方向に-1をかけることで反転させるのでメンバとして保持しておく
    /// </summary>
    private int _prevRandomDir;

    /// <summary>
    /// GameObjectを生成するためにAwakeのタイミングで呼び出す必要がある
    /// わざわざこうしているのはSerializable属性を付けて見た目を統一させるため
    /// </summary>
    public void InitOnAwake()
    {
        _searchWaypoint = new("SearchWaypoint");
        _forwardWaypoint = new("ForwardWaypoint");
        _prevRandomDir = (int)Mathf.Sign(Random.Range(-100, 100));
    }

    public void UpdateFootPos(Vector3 pos) => _footPos = pos;

    /// <summary>
    /// Search状態の移動をする際に呼ばれる
    /// Transformを返すことで移動中に移動先を動かすことが出来る
    /// </summary>
    public Transform GetSearchWaypoint(float distance, bool useRandomDistance)
    {
        if (Random.value <= 0.7f)
        {
            _prevRandomDir *= -1;
        }
        else
        {
            _prevRandomDir = (int)Mathf.Sign(Random.Range(-100, 100));
        }

        float percentage = useRandomDistance ? Random.value : 1;
        Vector3 targetPos = Vector3.right * _prevRandomDir * distance * percentage;
        Vector3 pos = _footPos + targetPos;
        _searchWaypoint.transform.position = pos;

        return _searchWaypoint.transform;
    }

    /// <summary>
    /// 前方に移動をする際に呼ばれる
    /// Transformを返すことで移動中に移動先を動かすことが出来る
    /// </summary>
    public Transform GetForwardWaypoint(float destination)
    {
        // FootPosの更新が一定間隔なので更新と更新の間に高いところから落ちると
        // 段差の上にFootPosがある状態でこのメソッドが呼ばれてしまうかもしれない
        // そうした場合、壁に引っかかるなど意図しない挙動をする可能性がある

        _footPos.x += destination;
        _forwardWaypoint.transform.position = _footPos;
        
        return _forwardWaypoint.transform;
    }
}
