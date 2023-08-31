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

namespace TwoD
{
	[DisallowMultipleComponent]
	public class Drop : MonoBehaviour
	{
		#region Public variables
		public GameObject toDrop;
		#endregion
		#region Private variables
		[SerializeField]
		private Vector2 spawnOffset = Vector2.zero;
		#endregion
		#region Public methods
		public void DoDrop()
		{
			Instantiate(toDrop, transform.TransformPoint(spawnOffset), Quaternion.identity);
		}
		#endregion
	}
}
