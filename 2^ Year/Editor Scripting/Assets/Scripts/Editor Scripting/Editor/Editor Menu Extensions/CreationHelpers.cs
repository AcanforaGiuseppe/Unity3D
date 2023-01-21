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
	public static class CreationHelpers
	{
		#region Private methods
		[MenuItem("GameObject/Create Default Hierarchy")]
		private static void DoCreateDefaultSceneHierarchy()
		{
			//	Create root
			GameObject sceneGO = new GameObject("Scene");
			//	Register action in undo stack
			Undo.RegisterCreatedObjectUndo(sceneGO, "Created scene hierarchy");
			//	Create environment hierarchy
			GameObject environmentGO = new GameObject("Environment");
			environmentGO.transform.SetParent(sceneGO.transform);
			new GameObject("Lighting").transform.SetParent(environmentGO.transform);
			new GameObject("Post").transform.SetParent(environmentGO.transform);
			new GameObject("Static").transform.SetParent(environmentGO.transform);
			new GameObject("Dynamic").transform.SetParent(environmentGO.transform);
			//	Create logic hierarchy
			GameObject logicGO = new GameObject("Logic");
			logicGO.transform.SetParent(sceneGO.transform);
			new GameObject("Level Manager").transform.SetParent(logicGO.transform);
			new GameObject("Cameras").transform.SetParent(logicGO.transform);
			new GameObject("Doors").transform.SetParent(logicGO.transform);
			new GameObject("AI").transform.SetParent(logicGO.transform);
		}
		[MenuItem("GameObject/3D Object/Box Trigger Volume")]
		private static void DoCreate3DBoxTriggerVolume()
		{
			GameObject volumeGO = new GameObject("Box Trigger Volume");
			//	Register action in undo stack
			Undo.RegisterCreatedObjectUndo(volumeGO, "Created Box Trigger Volume");
			BoxCollider volumeCol = volumeGO.AddComponent<BoxCollider>();
			volumeCol.isTrigger = true;
			volumeCol.center = new Vector3(0.0f, volumeCol.size.y * 0.5f, 0.0f);
			if(Selection.activeGameObject != null)
				volumeGO.transform.SetParent(Selection.activeGameObject.transform, false);
			Selection.activeGameObject = volumeGO;
		}
		#endregion
	}
}
