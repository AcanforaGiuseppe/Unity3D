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
	public class SideScrollCamera : MonoBehaviour
	{
		#region Public variables
		public Transform target;
		[Range(0.001f, 1.0f)]
		public float hardness = 0.2f;
		#endregion
		#region Lifecycle
		void Start()
		{
			AlignToTarget(1.0f);
		}
		void FixedUpdate()
		{
			AlignToTarget(hardness);
		}
		#endregion
		#region Private methods
		private void AlignToTarget(float hardness)
		{
			if(target == null)
				return;
			Vector3 pos = transform.position;
			pos.x = Mathf.Lerp(pos.x, target.position.x, hardness);
			transform.position = pos;
		}
		#endregion
	}
}
