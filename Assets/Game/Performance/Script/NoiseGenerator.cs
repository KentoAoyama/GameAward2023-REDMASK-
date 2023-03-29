using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("�m�C�Y�̃T�C�Y")]
    private Vector2Int _noiseSize = new Vector2Int(32, 32);
    [SerializeField, Range(0.0f, 0.1f), Tooltip("�����_���e�N�X�`���̍X�V����̊Ԋu")]
    private float _interval = 0;

    /// <summary>GlichTex��ID</summary>
    private int _glichTexId = Shader.PropertyToID("_GlitchTex");
    /// <summary>�m�C�Y��Texture</summary>
    private Texture2D _noiseTex;
    /// <summary>�e��m��</summary>
    private float _probability = 0.85f;
    /// <summary>�^�C�}�[</summary>
    private float _timer = default;

    private void Awake()
    {
        Init();
    }

    /// <summary>_noiseTex�̏������ƃm�C�Y�̐���</summary>
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
        // �w�肳�ꂽinterval�Ńm�C�Y���A�b�v�f�[�g���邩�̔�����s��
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

    /// <summary>�����_���ȐF��Ԃ�����</summary>
    /// <returns>�����_���ȐF</returns>
    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    /// <summary>�m�C�Y�e�N�X�`�����A�b�v�f�[�g���A�V�F�[�_�[�̕ϐ��Ɋi�[���Ă���</summary>
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

        // �V�F�[�_�[�̕ϐ��ɃZ�b�g����
        Shader.SetGlobalTexture(_glichTexId, _noiseTex);
    }
}
