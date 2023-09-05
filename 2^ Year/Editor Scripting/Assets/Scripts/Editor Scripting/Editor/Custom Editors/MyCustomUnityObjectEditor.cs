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

namespace UnityEditor.CustomEditors
{
	using EditorScripting.Data;
	using System;

	[CustomEditor(typeof(MyCustomUnityObject))]
	public class MyCustomUnityObjectEditor : Editor
	{
		#region Lifecycle
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			MyCustomUnityObject myCustomUnityObject = target as MyCustomUnityObject;

			bool wasGUIEnabled = GUI.enabled;
			GUI.enabled &= myCustomUnityObject != null;
			GUILayout.Space(15);
			using(new EditorGUILayout.HorizontalScope())
			{
				if(GUILayout.Button("Calculate Area"))
					CalculateArea(myCustomUnityObject);
				if(GUILayout.Button("Calculate Volume"))
					CalculateVolume(myCustomUnityObject);
			}
			GUI.enabled = wasGUIEnabled;
		}
		#endregion
		#region Private static methods
		private static void CalculateArea(in MyCustomUnityObject myCustomUnityObject)
		{
			float width = myCustomUnityObject.widthHeight.x;
			float height = myCustomUnityObject.widthHeight.y;
			float area = width * height;
			EditorUtility.DisplayDialog("My Custom Unity Object", $"The area of the rectangle of size {width}m x {height}m is {area}m^2", "Close");
		}
		private static void CalculateVolume(in MyCustomUnityObject myCustomUnityObject)
		{
			float width = myCustomUnityObject.widthHeightDepth.x;
			float height = myCustomUnityObject.widthHeightDepth.y;
			float depth = myCustomUnityObject.widthHeightDepth.z;
			float volume = width * height * depth;
			EditorUtility.DisplayDialog("My Custom Unity Object", $"The area of the prism of size {width}m x {height}m x {depth}m is {volume}m^3", "Close");
		}
		#endregion
	}
}
