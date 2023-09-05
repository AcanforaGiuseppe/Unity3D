using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyDataObject))]
public class MyDataObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MyDataObject data = target as MyDataObject;

        EditorGUILayout.HelpBox("Ciao", MessageType.None);
        EditorGUILayout.HelpBox("Ciao", MessageType.Info);
        EditorGUILayout.HelpBox("Ciao", MessageType.Warning);
        EditorGUILayout.HelpBox("Ciao", MessageType.Error);

        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Calc Area"))
                EditorUtility.DisplayDialog("MyDataObject inspector", $"Area is {data.widthHeight.x * data.widthHeight.y:0.##}", "Ok");
            if (GUILayout.Button("Calc Volume"))
                EditorUtility.DisplayDialog("MyDataObject inspector", $"Volume is {data.widthHeightDepth.x * data.widthHeightDepth.y * data.widthHeightDepth.z:0.##}", "Ok");
        }

    }
}