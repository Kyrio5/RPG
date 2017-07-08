using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumerateArrayAttribute))]
public class EnumerateArrayDrawer : PropertyDrawer
{
    #region Exposed

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int indentLevel = EditorGUI.indentLevel;
        int num = indentLevel - property.depth;

        SerializedProperty serializedProperty = property.Copy();
        SerializedProperty endProperty = serializedProperty.GetEndProperty();
        position.height = base.GetPropertyHeight(serializedProperty, label);
        EditorGUI.indentLevel = serializedProperty.depth + num - 1;

        serializedProperty.isExpanded = EditorGUI.Foldout(position, serializedProperty.isExpanded, label) && serializedProperty.hasVisibleChildren;

        position.y += position.height;
        int propertyIndex = -1;
        while (serializedProperty.NextVisible(serializedProperty.isExpanded) && !SerializedProperty.EqualContents(serializedProperty, endProperty))
        {
            EditorGUI.indentLevel = serializedProperty.depth + num;
            position.height = EditorGUI.GetPropertyHeight(serializedProperty, null, false);
            EditorGUI.BeginChangeCheck();
            if (propertyIndex == -1
                || propertyIndex >= enumNames.Length)
            {
                EditorGUI.PropertyField(position, serializedProperty);
            }
            else
            {
                EditorGUI.PropertyField(position, serializedProperty, new GUIContent(enumNames[propertyIndex]));
            }
            if (EditorGUI.EndChangeCheck())
            {
                break;
            }
            position.y += position.height;
            ++propertyIndex;
        }

        EditorGUI.indentLevel = indentLevel;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float extraHeight = 2.0f;

        if (!property.isExpanded)
        {
            return base.GetPropertyHeight(property, label) + extraHeight;
        }

        float baseHeight = base.GetPropertyHeight(property, label);

        return baseHeight * 2f + property.arraySize * baseHeight + extraHeight;
    }

    #endregion

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    #region Internal

    private string[] _enumNames = null;


    private string[] enumNames
    {
        get { return _enumNames != null ? _enumNames : _enumNames = Enum.GetNames((attribute as EnumerateArrayAttribute).enumType); }
    }


    #endregion
}
