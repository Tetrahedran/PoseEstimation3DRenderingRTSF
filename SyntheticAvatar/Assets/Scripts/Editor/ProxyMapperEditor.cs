using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProxyMapper))]
public class ProxyMapperEditor : Editor
{
    SerializedProperty bone;
    SerializedProperty staticProxy;
    SerializedProperty dynamicProxy;

    private void OnEnable()
    {
        bone = serializedObject.FindProperty("bone");
        staticProxy = serializedObject.FindProperty("staticProxy");
        dynamicProxy = serializedObject.FindProperty("dynamicProxy");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(bone, new GUIContent("Related bone"));
        EditorGUILayout.PropertyField(staticProxy);
        EditorGUILayout.PropertyField(dynamicProxy);

        serializedObject.ApplyModifiedProperties();
    }
}
