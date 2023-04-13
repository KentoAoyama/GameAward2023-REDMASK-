using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{

    [Header("オブジェクトを消すまでの時間")]
    [Tooltip("オブジェクトを消すまでの時間"), SerializeField]
    private float _destroyTime;

    [Header("ダメージを呼び出せるレイヤー")]
    [Tooltip("ダメージを呼び出せるレイヤー"), SerializeField]
    private LayerMask _layer;

    [Header("爆発範囲の取り方、箱型or円形")]
    [Tooltip("爆発範囲の取り方、箱型or円形"), SerializeField]
    private ShapeType _checkType = ShapeType.Box;

    [Header("箱型の判定の中心")]
    [Tooltip("箱型の判定の中心"), SerializeField]
    private Vector2 _boxCenterOffSet;

    [Header("箱型の判定の大きさ")]
    [Tooltip("箱型の判定の大きさ"), SerializeField]
    private Vector2 _boxSize;

    [Header("円形の判定の中心")]
    [Tooltip("円形の判定の中心"), SerializeField]
    private Vector2 _circleCenterOffSet;

    [Header("円形の判定の大きさ")]
    [Tooltip("円形の判定の大きさ"), SerializeField]
    private float _circleRadius;

    [Header("ドラム缶の画像用の子オブジェクト")]
    [Tooltip("ドラム缶の画像用の子オブジェクト"), SerializeField]
    private GameObject _image;

    [Header("爆発のエフェクト")]
    [Tooltip("爆発のエフェクト"), SerializeField]
    private ParticleSystem _particleSystem;

    private float _countDestriyTime;

    /// <summary>爆発したかどうか</summary>
    private bool _isExprosion = false;

    private enum ShapeType
    {
        //円形の判定
        Circle,
        //箱型の判定
        Box
    }

    void Update()
    {
        CountDestroyTime();
    }

    private void OnDrawGizmosSelected()
    {
        if (_checkType == Drum.ShapeType.Box)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + _boxCenterOffSet, _boxSize);
        }
        else if (_checkType == Drum.ShapeType.Circle)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere((Vector2)transform.position + _circleCenterOffSet, _circleRadius);
        }
    }

    /// <summary>範囲内のIDamagebleの取得/実行を試みる</summary>
    private void CallDamage()
    {
        //箱型で判定する場合
        if (_checkType == Drum.ShapeType.Box)
        {
            //範囲内をチェックする
            Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)transform.position + _boxCenterOffSet, _boxSize, 0, _layer);

            //取得したコライダーに対して、IDamagebleの取得/実行を試みる
            foreach (var hit in hits)
            {
                //IDamagebleの取得を試みる
                hit.TryGetComponent<IDamageable>(out IDamageable damageable);

                //IDamagebleの実行を試みる
                damageable?.Damage();
            }

        }   //円形で判定する場合
        else if (_checkType == Drum.ShapeType.Circle)
        {
            //範囲内をチェックする
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _circleCenterOffSet, _circleRadius, _layer);

            //取得したコライダーに対して、IDamagebleの取得/実行を試みる
            foreach (var hit in hits)
            {
                //IDamagebleの取得を試みる
                hit.TryGetComponent<IDamageable>(out IDamageable damageable);

                //IDamagebleの実行を試みる
                damageable?.Damage();
            }
        }

    }

    /// <summary>インターフェイスの関数。爆発開始処理</summary>
    public void Damage()
    {
        //既に爆発していたら呼ばない。2度呼ばれないようにしている。
        if (_isExprosion) return;

        //範囲内にいる物に対してダメージ処理を実行
        CallDamage();

        //ドラム缶の画像を消す
        _image.SetActive(false);

        //爆発のエフェクトを再生
        _particleSystem.Play();

        _isExprosion = true;

        Debug.Log("D");
    }

    /// <summary>爆発してからオブジェクトを消すまでの時間を計測する関数</summary>
    public void CountDestroyTime()
    {
        if (!_isExprosion) return;

        //時間を計測する
        _countDestriyTime += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        //消すまでの時間が立ったら消す
        if (_countDestriyTime > _destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
