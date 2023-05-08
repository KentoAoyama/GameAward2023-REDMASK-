using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableOutline : MonoBehaviour
{
    [SerializeField]
    private Shader _outlineShader;

    [SerializeField]
    private float _outlineRange = 7.0f;

    [SerializeField]
    private Color _outlineColor = Color.yellow;

    private Image _image;
    private int _rangePropertyId = Shader.PropertyToID("_OutlineRange");
    private int _colorPropertyId = Shader.PropertyToID("_OutlineColor");

    private void Awake()
    {
        _image = GetComponent<Image>();

        _image.material = new Material(_outlineShader);
        _image.material.SetFloat(_rangePropertyId, 0.0f);
        _image.material.SetColor(_colorPropertyId,  _outlineColor);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _image.material.SetFloat(_rangePropertyId, _outlineRange);
        }
        else if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            _image.material.SetFloat(_rangePropertyId, 0.0f);
        }
    }
}
