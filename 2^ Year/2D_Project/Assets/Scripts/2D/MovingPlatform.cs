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
	public class MovingPlatform : MonoBehaviour
	{
		#region Public variables
		#endregion
		#region Private variables
		[SerializeField]
		private Vector3[] waypoints = new Vector3[]{ new Vector3(-3.0f, 0.0f, 0.0f), new Vector3(3.0f, 0.0f, 0.0f) };
		private Vector3[] waypointsWS = null;
		[SerializeField]
		[Range(0.1f, 15.0f)]
		private float speed = 3.0f;
		[SerializeField]
		[Range(0.0f, 5.0f)]
		private float capWait = 0.75f;
		private int waypointID = 0;
		private int indexDirection = 1;
		private Vector3 moveDirection;
		private Vector3 distanceFromWaypoint;
		private float lastCapReachedTime = 0.0f;
		#endregion
		#region Lifecycle
		void Awake()
		{
			waypointsWS = waypoints.Select<Vector3, Vector3>(ls => transform.TransformPoint(ls)).ToArray<Vector3>();
		}
		void Start()
		{
			transform.position = waypointsWS[0];
			lastCapReachedTime = Time.time;
			IncrementIndex();
		}
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(
				collision.enabled &&
				collision.collider.CompareTag("Player") &&
				Vector2.Dot(Vector2.down, collision.GetContact(0).normal) > 0.85f
			)
				collision.transform.SetParent(transform);
		}
		void OnCollisionExit2D(Collision2D collision)
		{
			if(collision.collider.CompareTag("Player"))
				collision.transform.SetParent(null);

		}
		void FixedUpdate()
		{
			HandleMovement();
		}
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if(EditorApplication.isPlaying)
			{
				if(waypointsWS != null)
					for(int i = 1; i < waypointsWS.Length; i++)
						Gizmos.DrawLine(waypointsWS[i - 1], waypointsWS[i]);
			}
			else
			{
				if(waypoints != null)
					for(int i = 1; i < waypoints.Length; i++)
						Gizmos.DrawLine(transform.TransformPoint(waypoints[i - 1]), transform.TransformPoint(waypoints[i]));
			}
		}
#endif
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		private void IncrementIndex()
		{
			waypointID += indexDirection;
			if(
				waypointID < 0 ||
				waypointID >= waypoints.Length
			)
			{
				indexDirection *= -1;
				waypointID += 2 * indexDirection;
				lastCapReachedTime = Time.time;
			}
			distanceFromWaypoint = waypointsWS[waypointID] - transform.position;
			moveDirection = distanceFromWaypoint.normalized;
		}
		private void HandleMovement()
		{
			//	Wait when on caps
			if(Time.time - lastCapReachedTime < capWait)
				return;
			//	Calculate desired movement offset
			Vector3 offset = moveDirection * speed * Time.fixedDeltaTime;
			distanceFromWaypoint = waypointsWS[waypointID] - transform.position;
			//	Check overshooting or add offset to position
			if(offset.sqrMagnitude > distanceFromWaypoint.sqrMagnitude)
			{
				transform.position = waypointsWS[waypointID];
				IncrementIndex();
			}
			else
				transform.position += offset;
		}
		#endregion
	}
}
