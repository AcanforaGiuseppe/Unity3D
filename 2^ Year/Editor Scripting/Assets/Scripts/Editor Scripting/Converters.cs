using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Converters
{
    [MenuItem("GameObject/Convert/Skinned to Mesh", true)]
    private static bool CanConvertSkinnedToMesh()
    {
        foreach (GameObject go in Selection.gameObjects)
            if (go.GetComponentsInChildren<SkinnedMeshRenderer>().Length > 0)
                return true;

        return false;
    }

    [MenuItem("GameObject/Convert/Skinned to Mesh")]
    private static void DoConvertSkinnedToMesh()
    {
        List<SkinnedMeshRenderer> smrs = new List<SkinnedMeshRenderer>();

        // Find renderers
        foreach (GameObject go in Selection.gameObjects)
            foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
                if (!smrs.Contains(smr))
                    smrs.Add(smr);

        {
            // Convert
            int i;
            for (i = 0; i < smrs.Count; i++)
            {
                MeshFilter mf = Undo.AddComponent<MeshFilter>(smrs[i].gameObject);
                MeshRenderer mr = Undo.AddComponent<MeshRenderer>(smrs[i].gameObject);
                mf.sharedMesh = smrs[i].sharedMesh;
                mr.sharedMaterials = smrs[i].sharedMaterials;
            }

            // Destroy
            for (i = 0; i < smrs.Count; i++)
                Undo.DestroyObjectImmediate(smrs[i]);
        }
    }

}