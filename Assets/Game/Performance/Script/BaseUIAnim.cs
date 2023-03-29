using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways, RequireComponent(typeof(Graphic))]
public class BaseUIAnim : MonoBehaviour, IMaterialModifier
{
    /// <summary>アニメーションさせたマテリアルを渡すGraphic</summary>
    private Graphic _animGraphic;
    /// <summary>アニメーションをさせるマテリアル</summary>
    protected Material _material;
    /// <summary>アニメーションさせたマテリアルを渡すGraphic</summary>
    public Graphic AnimGraphic
    {
        get
        {
            //_animGraphicがnullの際にGetComponentする
            _animGraphic ??= GetComponent<Graphic>();
            return _animGraphic;
        }
    }

    /// <summary>指定のマテリアルにDirtyフラグが立った際に呼ばれる</summary>
    /// <param name="baseMaterial">Animationさせるマテリアル</param>
    /// <returns>変更されたマテリアル</returns>
    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return baseMaterial;
        }

        UpdateMaterial(baseMaterial);
        return _material;
    }

    /// <summary>プロパティがアニメーションした際にDirtyフラグを立てる</summary>
    private void OnDidApplyAnimationProperties()
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return;
        }

        _animGraphic.SetMaterialDirty();
    }

    /// <summary>マテリアルの値を変更するための関数</summary>
    /// <param name="baseMatrial">値が変化する前のMaterial</param>
    protected virtual void UpdateMaterial(Material baseMatrial)
    {
    }

    private void OnEnable()
    {
        if (!AnimGraphic)
        {
            return;
        }

        _animGraphic.SetMaterialDirty();
    }

    private void OnDisable()
    {
        if (!_material)
        {
            DestroyMaterial();
        }

        if (!AnimGraphic)
        {
            _animGraphic.SetMaterialDirty();
        }
    }

    public void DestroyMaterial()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            DestroyImmediate(_material);
            _material = null;
            return;
        }
#endif
        Destroy(_material);
        _material = null;
    }

    private void OnValidate()
    {
        if (!isActiveAndEnabled || !AnimGraphic)
        {
            return;
        }

        AnimGraphic.SetMaterialDirty();
    }
}
