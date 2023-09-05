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

namespace EditorScripting.Data
{
	using EditorScripting.Attributes;

	[CreateAssetMenu]
	public class MyCustomUnityObject : ScriptableObject
	{
		#region Sub-classes
		[S.Serializable]
		public class MinMax
		{
			public float min = 0.0f;
			public float max = 1.0f;
		}
		#endregion
		#region Public variables
		[MinMaxRange(0.0f, 50.0f)]
		public MinMax minMax = new MinMax{ min = 5.0f, max = 25.0f };
		public Vector2 widthHeight = new Vector2(2.0f, 1.0f);
		public Vector3 widthHeightDepth = new Vector3(2.0f, 1.0f, 0.75f);
		#endregion
	}
}
