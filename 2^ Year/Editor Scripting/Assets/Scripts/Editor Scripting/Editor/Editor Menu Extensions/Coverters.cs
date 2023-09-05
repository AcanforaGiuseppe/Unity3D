using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace UnityEditor.MenuExtensions
{
	public static class Converters
	{
		#region Private methods
		[MenuItem("Edit/Convert/SinnedMeshRenderer to MehRenderer", true)]
		private static bool CanConvertSkinnedMeshToMesh()
		{
			foreach(GameObject go in Selection.gameObjects)
				if(go.GetComponentsInChildren<SkinnedMeshRenderer>().Length > 0)
					return true;
			return false;
		}
		[MenuItem("Edit/Convert/SinnedMeshRenderer to MehRenderer", false)]
		private static void DoConvertSkinnedMeshToMesh()
		{
			//	Validate call
			if(!CanConvertSkinnedMeshToMesh())
				return;
			//	Convert SMR to MR
			foreach(GameObject go in Selection.gameObjects)
			{
				//	Register for undo
				SkinnedMeshRenderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
				for(int i = smrs.Length - 1; i >= 0; i--)
				{
					MeshFilter mf = Undo.AddComponent<MeshFilter>(smrs[i].gameObject);
					MeshRenderer mr = Undo.AddComponent<MeshRenderer>(smrs[i].gameObject);
					mf.sharedMesh = smrs[i].sharedMesh;
					mr.sharedMaterials = smrs[i].sharedMaterials;
					Undo.DestroyObjectImmediate(smrs[i]);
				}
			}
		}
		#endregion
	}
}