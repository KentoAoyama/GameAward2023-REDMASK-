using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("ノイズのサイズ")]
    private Vector2Int _noiseSize = new Vector2Int(32, 32);
    [SerializeField, Range(0.0f, 0.1f), Tooltip("ランダムテクスチャの更新判定の間隔")]
    private float _interval = 0;

    /// <summary>GlichTexのID</summary>
    private int _glichTexId = Shader.PropertyToID("_GlitchTex");
    /// <summary>ノイズのTexture</summary>
    private Texture2D _noiseTex;
    /// <summary>各種確率</summary>
    private float _probability = 0.85f;
    /// <summary>タイマー</summary>
    private float _timer = default;

    private void Awake()
    {
        Init();
    }

    /// <summary>_noiseTexの初期化とノイズの生成</summary>
    private void Init()
    {
        _noiseTex = new Texture2D(_noiseSize.x, _noiseSize.y, TextureFormat.RGBA32, false);
        _noiseTex.hideFlags = HideFlags.DontSave;
        _noiseTex.wrapMode = TextureWrapMode.Clamp;
        _noiseTex.filterMode = FilterMode.Point;

        UpdateNoise();
    }

    private void Update()
    {
        // 指定されたintervalでノイズをアップデートするかの判定を行う
        if (_timer >= _interval)
        {
            if (Random.value > _probability)
            {
                UpdateNoise();
            }

            _timer = 0;
        }

        _timer += Time.deltaTime;
    }

    /// <summary>ランダムな色を返す函数</summary>
    /// <returns>ランダムな色</returns>
    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    /// <summary>ノイズテクスチャをアップデートし、シェーダーの変数に格納している</summary>
    private void UpdateNoise()
    {
        var tempColor = RandomColor();

        for (var i = 0; i < _noiseTex.height; i++)
        {
            for (var j = 0; j < _noiseTex.width; j++)
            {
                if (Random.value > _probability)
                {
                    tempColor = RandomColor();
                }

                _noiseTex.SetPixel(i, j, tempColor);
            }
        }

        _noiseTex.Apply();

        // シェーダーの変数にセットする
        Shader.SetGlobalTexture(_glichTexId, _noiseTex);
    }
}
