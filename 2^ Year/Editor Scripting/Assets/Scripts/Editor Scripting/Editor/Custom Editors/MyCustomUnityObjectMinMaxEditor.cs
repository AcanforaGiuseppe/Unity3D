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
	using MinMax = EditorScripting.Data.MyCustomUnityObject.MinMax;

	[CustomPropertyDrawer(typeof(MinMax))]
	public class MyCustomUnityObjectMinMaxEditor : PropertyDrawer
	{
		#region Lifecycle
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//	Draw label
			Rect editPosition = EditorGUI.PrefixLabel(position, label);

			//	Calculate areas
			float halfColumnWidth = editPosition.width * 0.5f;
			float labelWidth = 35.0f;
			float editWidth = halfColumnWidth - labelWidth;
			Rect minLabelRect = new Rect(editPosition.x, editPosition.y, labelWidth, editPosition.height);
			Rect minRect = new Rect(editPosition.x + labelWidth, editPosition.y, editWidth, editPosition.height);
			Rect maxLabelRect = new Rect(editPosition.x + halfColumnWidth, editPosition.y, labelWidth, editPosition.height);
			Rect maxRect = new Rect(editPosition.x + halfColumnWidth + labelWidth, editPosition.y, editWidth, editPosition.height);

			//	Get reference to serialized properties
			SerializedProperty minProp = property.FindPropertyRelative("min");
			SerializedProperty maxProp = property.FindPropertyRelative("max");

			// Draw inspector
			EditorGUI.LabelField(minLabelRect, "min");
			minProp.floatValue = EditorGUI.FloatField(minRect, minProp.floatValue);
			EditorGUI.LabelField(maxLabelRect, "max");
			maxProp.floatValue = EditorGUI.FloatField(maxRect, maxProp.floatValue);
		}
		#endregion
	}
}
