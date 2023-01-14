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

namespace TwoD.Classroom
{
	[DisallowMultipleComponent]
	public class MovingPlatform : MonoBehaviour
	{
		#region Public variables
		public float speed = 0.2f;
		#endregion
		#region Private variables
		[SerializeField]
		private Vector3 startWS, endWS;
		#endregion
		#region Lifecycle
		void Awake()
		{

		}
		void Start()
		{

		}
		void Update()
		{
			float alpha = (Time.time * speed) % 2.0f;   // 0.0f -> 2.0f
			alpha -= 1.0f;  //	-1.0f -> 1.0f
			if(alpha < 0.0f)
				alpha = 1.0f - alpha;
			transform.position = Vector3.Lerp(startWS, endWS, alpha);
		}
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.collider.CompareTag("Player"))
				collision.transform.SetParent(transform);
		}
		void OnCollisionExit2D(Collision2D collision)
		{
			if(collision.collider.CompareTag("Player"))
				collision.transform.SetParent(null);
		}
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		#endregion
	}
}
