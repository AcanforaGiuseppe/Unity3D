using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuAdditions
{
    [MenuItem("GameObject/Create Hierarchy")]
    [MenuItem("My Menu/My Item")]
    private static void DoCreateHierarchy()
    {
        GameObject root = new GameObject("Scene");
        Undo.RegisterCreatedObjectUndo(root, "create hierarchy");
        new GameObject("Environment").transform.SetParent(root.transform);
        new GameObject("Logic").transform.SetParent(root.transform);
        new GameObject("Lighting").transform.SetParent(root.transform);
        new GameObject("Etc").transform.SetParent(root.transform);
    }

    [MenuItem("GameObject/3D Object/Box Trigger Volume")]
    private static void DoCreateBoxVolumeTrigger()
    {
        GameObject root = new GameObject("Box");
        Undo.RegisterCreatedObjectUndo(root, "create box volume trigger");
        BoxCollider box = Undo.AddComponent<BoxCollider>(root);
        box.isTrigger = true;
        box.center = new Vector3(0, box.size.y * 0.5f, 0);

        if (Selection.activeGameObject != null)
            root.transform.SetParent(Selection.activeGameObject.transform, false);

        Selection.activeGameObject = root;
    }

}