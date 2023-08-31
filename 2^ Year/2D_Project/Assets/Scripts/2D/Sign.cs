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
	[RequireComponent(typeof(Collider2D))]
	public class Sign : MonoBehaviour
	{
		#region Private sub-classes
		[S.Serializable]
		private sealed class SignEvent : UnityEngine.Events.UnityEvent<bool> { }
		#endregion
		#region Private variables
		[SerializeField]
		private SignEvent onSignVisibilityChanged;
		#endregion
		#region Lifecycle
		void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.CompareTag("Player"))
				onSignVisibilityChanged?.Invoke(true);
		}
		void OnTriggerExit2D(Collider2D collider)
		{
			if(collider.CompareTag("Player"))
				onSignVisibilityChanged?.Invoke(false);
		}
		#endregion
	}
}
