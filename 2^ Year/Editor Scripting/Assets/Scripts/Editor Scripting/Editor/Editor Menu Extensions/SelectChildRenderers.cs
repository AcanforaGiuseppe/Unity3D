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
	public static class SelectChildRenderers
	{
		#region Private methods
		[MenuItem("Edit/Select Child Renderers", true)]
		private static bool CanSelectChildRenderers()
		{
			if(Selection.gameObjects.Length < 1)
				return false;
			foreach(GameObject go in Selection.gameObjects)
				if(AssetDatabase.Contains(go))
					return false;
			return true;
		}
		[MenuItem("Edit/Select Child Renderers", false)]
		private static void DoSelectChildRenderers()
		{
			//	Prevent inconsistent calls
			if(!CanSelectChildRenderers())
				return;
			//	Find renderers
			List<GameObject> renderers = new List<GameObject>();
			foreach(GameObject go in Selection.gameObjects)
			{
				foreach(Renderer renderer in go.GetComponentsInChildren<Renderer>())
					if(!renderers.Contains(renderer.gameObject))
						renderers.Add(renderer.gameObject);
			}
			//	Update selection
			Selection.objects = renderers.ToArray();
		}
		#endregion
	}
}
