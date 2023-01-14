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
	public class SelfDestruct : MonoBehaviour
	{
		#region Private variables
		private float life = -1.0f;
		#endregion
		#region Lifecycle
		IEnumerator Start()
		{
			if(life > 0.0f)
			{
				yield return new WaitForSeconds(life);
				Die();
			}
		}
		#endregion
		#region Public methods
		public void Die()
		{
			Destroy(gameObject);
		}
		#endregion
	}
}
