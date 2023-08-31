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

namespace TwoD.Tileset
{
	[DisallowMultipleComponent]
	public class CameraFollowTarget : MonoBehaviour
	{
		#region Public variables
		public Transform target = null;
		[Range(0.001f, 1f)]
		public float followHardness = 0.2f;
		public bool initOnTarget = true;
		#endregion
		#region Lifecycle
		void Start()
		{
			if(
				initOnTarget &&
				target != null
			)
				transform.position = target.position;
		}
		void Update()
		{
			if(target != null)
				transform.position = Vector3.Lerp(transform.position, target.position, followHardness);
		}
		#endregion
	}
}
