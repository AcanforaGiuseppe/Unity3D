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
	public class Box : MonoBehaviour
	{
		#region Private variables
		[SerializeField]
		private int maxActivations = 1;
		private int activationsCount = 0;
		[SerializeField]
		private UnityEngine.Events.UnityEvent onActivated;
		#endregion
		#region Lifecycle
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(
				collision.collider.CompareTag("Player") &&
				Vector2.Dot(collision.GetContact(0).normal, Vector2.up) > 0.85f
			)
				Trigger();
		}
		#endregion
		#region Private methods
		private void Trigger()
		{
			if(
				maxActivations > -1 &&
				activationsCount >= maxActivations
			)
				return;
			activationsCount++;
			onActivated?.Invoke();
		}
		#endregion
	}
}
