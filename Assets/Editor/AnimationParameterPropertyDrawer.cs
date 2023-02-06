using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationParameterAttribute))]
public class AnimationParameterPropertyDrawer : PropertyDrawer
{
    private int _index = -1;
    private GUIContent[] _parameterNames = default;
    private bool isSuccessSetUp = default;
    private string[] _strings;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var oldstrings = _strings;
        _strings = GetElements(property);
        if (_index == -1 || !_strings.Equals(oldstrings) || !isSuccessSetUp)
        {
            isSuccessSetUp = SetUp(position, property, label);
        }
        if (isSuccessSetUp)
        {
            int oldIndex = _index;
            _index = EditorGUI.Popup(position, label, _index, _parameterNames);

            if (oldIndex != _index)
            {
                property.stringValue = _parameterNames[_index].text;
            }
        }
    }

    private bool SetUp(Rect position, SerializedProperty property, GUIContent label)
    {
        // アニメーターを取得
        Animator animator = (property.serializedObject.targetObject as Component).GetComponent<Animator>();
        if (animator == null)
        {
            EditorGUI.LabelField(position, "AnimationParameter属性が使用されていますが、AnimatorComponentがアタッチされていません");
            return false; // 取得に失敗したら処理を終了する。
        }

        // アニメーションコントローラを取得
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController == null)
        {
            EditorGUI.LabelField(position, "AnimationParameter属性が使用されていますが、AnimatorControllerがアサインされていません。");
            return false;
        }

        // パラメータ名の配列を作成
        string[] parameterNames = animatorController.parameters.Select(p => p.name).ToArray();
        if (parameterNames.Length == 0)
        {
            EditorGUI.LabelField(position, "AnimationParameter属性が使用されていますが、parameter が一つも設定されていません。");
            return false;
        }

        // GUIに描画するためのコンテントを作成
        _parameterNames = new GUIContent[parameterNames.Length];

        // コンテントに名前を設定する。
        for (int i = 0; i < parameterNames.Length; i++)
        {
            _parameterNames[i] = new GUIContent($"{parameterNames[i]}");
        }

        // 再設定用プログラム
        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool sceneNameFound = false;
            for (int i = 0; i < _parameterNames.Length; i++)
            {
                if (_parameterNames[i].text == property.stringValue)
                {
                    _index = i;
                    sceneNameFound = true;
                    break;
                }
            }
            if (!sceneNameFound)
                _index = 0;
        }
        else _index = 0;

        // プロパティに値を設定する。
        property.stringValue = _parameterNames[_index].text;

        return true;
    }

    private string[] GetElements(SerializedProperty property)
    {
        // アニメーターを取得
        Animator animator = (property.serializedObject.targetObject as Component).GetComponent<Animator>();
        if (animator == null)
        {
            return new string[0];
        }

        // アニメーションコントローラを取得
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController == null)
        {
            return new string[0];
        }

        // パラメータ名の配列を作成
        return animatorController.parameters.Select(p => p.name).ToArray();
    }
}