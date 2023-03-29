using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways, RequireComponent(typeof(Graphic))]
public class BaseUIAnim : MonoBehaviour, IMaterialModifier
{
    /// <summary>�A�j���[�V�����������}�e���A����n��Graphic</summary>
    private Graphic _animGraphic;
    /// <summary>�A�j���[�V������������}�e���A��</summary>
    protected Material _material;
    /// <summary>�A�j���[�V�����������}�e���A����n��Graphic</summary>
    public Graphic AnimGraphic
    {
        get
        {
            //_animGraphic��null�̍ۂ�GetComponent����
            _animGraphic ??= GetComponent<Graphic>();
            return _animGraphic;
        }
    }

    /// <summary>�w��̃}�e���A����Dirty�t���O���������ۂɌĂ΂��</summary>
    /// <param name="baseMaterial">Animation������}�e���A��</param>
    /// <returns>�ύX���ꂽ�}�e���A��</returns>
    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return baseMaterial;
        }

        UpdateMaterial(baseMaterial);
        return _material;
    }

    /// <summary>�v���p�e�B���A�j���[�V���������ۂ�Dirty�t���O�𗧂Ă�</summary>
    private void OnDidApplyAnimationProperties()
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return;
        }

        _animGraphic.SetMaterialDirty();
    }

    /// <summary>�}�e���A���̒l��ύX���邽�߂̊֐�</summary>
    /// <param name="baseMatrial">�l���ω�����O��Material</param>
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
