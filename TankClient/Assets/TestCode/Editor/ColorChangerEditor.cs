using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorChanger))]
public class ColorChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ColorChanger t = target as ColorChanger;
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("�÷� ����"))
        {
            t.ColorChange();
        };

        if (GUILayout.Button("���� �÷�"))
        {
            t.ResetColor();
        }
        GUILayout.EndHorizontal();

    }
}
