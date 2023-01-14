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
	[RequireComponent(typeof(Rigidbody2D))]
	public class EnemyPatrol : MonoBehaviour
	{
		#region Public variables
		public Transform patrolStart;
		public Transform patrolEnd;
		public float minSpeed = 0.2f;
		public float maxSpeed = 0.5f;
		#endregion
		#region Private variables
		private Rigidbody2D rb = null;
		private bool seekEnd = true;
		#endregion
		#region Private properties
		private Vector3 target => seekEnd ? patrolEnd.position : patrolStart.position;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}
		void FixedUpdate()
		{
			Vector2 velocity = target - transform.position;
			float speed = velocity.magnitude;
			velocity = Vector2.ClampMagnitude(velocity.normalized * Mathf.Max(minSpeed, speed), maxSpeed);
			CheckTargetReahed(ref velocity);
			rb.velocity = velocity;
		}
		#endregion
		#region Private methods
		private bool CheckTargetReahed(ref Vector2 offset)
		{
			if(Vector2.Distance(transform.position, target) <= (offset * Time.fixedDeltaTime).magnitude)
			{
				offset *= -1;
				seekEnd = !seekEnd;
				return true;
			}
			return false;
		}
		#endregion
	}
}
