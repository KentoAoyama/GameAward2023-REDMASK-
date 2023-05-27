using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableOutline : MonoBehaviour
{
    [SerializeField]
    private Shader _outlineShader;
    [Header("Outline")]
    [SerializeField]
    private float _outlineRange = 7.0F;

    [SerializeField, ColorUsage(true, true)]
    private Color _outlineColor = Color.yellow;
    [Header("Dissolve")]
    [SerializeField]
    private  Texture _dissolveTex = default;
    [SerializeField, Range(0.0F, 1.0F)]
    private float _dissolveAmount = 0.5F;
    [SerializeField, Range(0.0F, 1.0F)]
    private float _dissolveRange = 0.15F;
    [SerializeField, ColorUsage(true, true)]
    private Color _dissolveColor = Color.yellow;
    [Header("Scroll")]
    [SerializeField]
    private Vector2 _scroll = default;
    [SerializeField]
    private float _scrollStrength = 1.0F;

    private Image _image;
    private int _outlineRangePropertyId = Shader.PropertyToID("_OutlineRange");
    private int _outlineColorPropertyId = Shader.PropertyToID("_OutlineColor");
    private int _dissolveTexId = Shader.PropertyToID("_DissolveTex");
    private int _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    private int _dissolveRangeId = Shader.PropertyToID("_DissolveRange");
    private int _dissolveColorId = Shader.PropertyToID("_DissolveColor");
    private int _scrollId = Shader.PropertyToID("_Scroll");
    private int _scrollStrengthId = Shader.PropertyToID("_ScrollStrength");

    private void Awake()
    {
        _image = GetComponent<Image>();

        _image.material = new Material(_outlineShader);
        _image.material.renderQueue = 3000;
        _image.material.SetFloat(_outlineRangePropertyId, 0.0f);
        _image.material.SetColor(_outlineColorPropertyId,  _outlineColor);
        _image.material.SetTexture(_dissolveTexId, _dissolveTex);
        _image.material.SetFloat(_dissolveAmountId, _dissolveAmount);
        _image.material.SetFloat(_dissolveRangeId, _dissolveRange);
        _image.material.SetColor(_dissolveColorId, _dissolveColor);
        _image.material.SetVector(_scrollId, new Vector4(_scroll.x, _scroll.y, 0, 0));
        _image.material.SetFloat(_scrollStrengthId, _scrollStrength);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _image.material.SetFloat(_outlineRangePropertyId, _outlineRange);
        }
        else if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            _image.material.SetFloat(_outlineRangePropertyId, 0.0f);
        }

        _image.material.SetColor(_outlineColorPropertyId,  _outlineColor);
        _image.material.SetTexture(_dissolveTexId, _dissolveTex);
        _image.material.SetFloat(_dissolveAmountId, _dissolveAmount);
        _image.material.SetFloat(_dissolveRangeId, _dissolveRange);
        _image.material.SetColor(_dissolveColorId, _dissolveColor);
        _image.material.SetVector(_scrollId, new Vector4(_scroll.x, _scroll.y, 0, 0));
        _image.material.SetFloat(_scrollStrengthId, _scrollStrength);
    }
}
