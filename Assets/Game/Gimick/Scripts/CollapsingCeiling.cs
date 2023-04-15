using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingCeiling : MonoBehaviour,IDamageable
{
    [Header("オブジェクトを消すまでの時間")]
    [Tooltip("オブジェクトを消すまでの時間"), SerializeField]
    private float _destroyTime;

    [Header("ダメージを呼び出せるレイヤー")]
    [Tooltip("ダメージを呼び出せるレイヤー"), SerializeField]
    private LayerMask _layer;



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

    [Header("天井の画像用の子オブジェクト")]
    [Tooltip("天井の画像用の子オブジェクト"), SerializeField]
    private GameObject _image;

    [Header("瓦礫の画像用の子オブジェクト")]
    [Tooltip("瓦礫の画像用の子オブジェクト"), SerializeField]
    private GameObject _rubbleImage;


    [Header("瓦礫のエフェクト")]
    [Tooltip("瓦礫のエフェクト"), SerializeField]
    private ParticleSystem _particleSystem;

    [Header("煙のエフェクト")]
    [Tooltip("煙のエフェクト"), SerializeField]
    private ParticleSystem _smokeParticleSystem;

    private float _countDestriyTime;

    /// <summary>爆発したかどうか</summary>
    private bool _isBrake = false;

    void Update()
    {
        CountDestroyTime();
    }

    private void OnDrawGizmosSelected()
    {

    }


    /// <summary>瓦礫が地面に落ちたかどうかを確認する</summary>
    private void CheckDownRubble()
    {


    }


    /// <summary>インターフェイスの関数。爆発開始処理</summary>
    public void Damage()
    {
        //既に爆発していたら呼ばない。2度呼ばれないようにしている。
        if (_isBrake) return;

        //天井の画像を消す
        _image.SetActive(false);

        //崩れるのエフェクトを再生
        _particleSystem.Play();

        _isBrake = true;

        Debug.Log("D");
    }

    /// <summary>爆発してからオブジェクトを消すまでの時間を計測する関数</summary>
    public void CountDestroyTime()
    {
        if (!_isBrake) return;

        //時間を計測する
        _countDestriyTime += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        //消すまでの時間が立ったら消す
        if (_countDestriyTime > _destroyTime)
        {
            //煙を出す
            _smokeParticleSystem.Play();
            //瓦礫を画像を出す
            _rubbleImage.SetActive(true);

            //瓦礫のパーティクルを消す
            Destroy(_particleSystem);
        }
    }
}
