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
	public class SimpleParallax : MonoBehaviour
	{
		#region Public variables
		#endregion
		#region Private variables
		[SerializeField]
		private Transform reference;
		[SerializeField]
		private float parallaxRatio = 0.2f;
		private Vector3 restPosition;
		private Vector3 referenceRestPosition;
		#endregion
		#region Lifecycle
		void Awake()
		{

		}
		void Start()
		{
			restPosition = transform.position;
			referenceRestPosition = reference.position;
		}
		void Update()
		{
			transform.position = restPosition + (reference.position - referenceRestPosition) * parallaxRatio;
		}
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		#endregion
	}
}
