using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.AssetImporters;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor.CustomImporters
{
    [ScriptedImporter(1, "aivm")]
    public class AIVMeshImporter : ScriptedImporter
    {
        #region Private variables
        [SerializeField]
        private Material material = null;
        #endregion

        #region Lifecycle
        public override void OnImportAsset(AssetImportContext ctx)
        {
            //	Read data
            string[] lines = File.ReadAllText(ctx.assetPath).Split(new string[] { "\r", "\n", "\r\n" }, S.StringSplitOptions.RemoveEmptyEntries).Where<string>(line => !line.StartsWith("#") && !string.IsNullOrWhiteSpace(line)).ToArray<string>();

            //	Validate data
            if (lines.Length != 2)
                throw new S.Exception("Malformed mesh (only two sets of coordinates expected), failed to import.");

            //	Extrapolate vertices and triangles
            int[] vertsCoords = lines[0].Split(new char[] { ',' }).Select<string, int>(s => int.Parse(s.Trim())).ToArray<int>();
            int[] trisIDs = lines[1].Split(new char[] { ',' }).Select<string, int>(s => int.Parse(s.Trim())).ToArray<int>();

            //	Validate extrapolated data
            if (vertsCoords.Length % 3 != 0)
                throw new S.Exception("Malformed mesh (vertices not multiples of 3), failed to import.");

            if (trisIDs.Length % 3 != 0)
                throw new S.Exception("Malformed mesh (triangles vertices not multiples of 3), failed to import.");

            //	Build assets based on data
            string assetName = Path.GetFileNameWithoutExtension(assetPath);

            Mesh aivMesh = new Mesh();
            aivMesh.name = $"{assetName}_Mesh";
            aivMesh.vertices = AssembleCoordinates(vertsCoords);
            aivMesh.triangles = trisIDs;

            GameObject aivMeshAsset = new GameObject(assetName);

            MeshFilter aivMeshFilter = aivMeshAsset.AddComponent<MeshFilter>();
            aivMeshFilter.sharedMesh = aivMesh;
            MeshRenderer aivMeshRenderer = aivMeshAsset.AddComponent<MeshRenderer>();

            aivMeshRenderer.sharedMaterial = material;

            //	Add assets to context
            ctx.AddObjectToAsset(aivMeshAsset.name, aivMeshAsset);
            ctx.AddObjectToAsset(aivMesh.name, aivMesh);
            ctx.SetMainObject(aivMeshAsset);
        }
        #endregion

        #region Private static methods
        private static Vector3[] AssembleCoordinates(int[] coordinatesCoponents)
        {
            Vector3[] coords = new Vector3[coordinatesCoponents.Length / 3];

            for (int i = 0; i < coords.Length; i++)
                coords[i] = new Vector3(coordinatesCoponents[i * 3 + 0], coordinatesCoponents[i * 3 + 1], coordinatesCoponents[i * 3 + 2]);

            return coords;
        }
        #endregion
    }
}