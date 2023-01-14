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
using System;

namespace TwoD.Classroom
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour
	{
		#region Public variables
		public float moveSpeed = 30.0f;
		public float jumpSpeed = 25.0f;
		public float maxJumpBoost = 0.35f;
		public float killY = -5.0f;
		public float maxWalkSpeed = 4.0f;
		public float maxFallSpeed = 4.0f;
		public bool ignoreMass = false;
		#endregion
		#region Private variables
		private Rigidbody2D rb2d = null;
		[SerializeField]
		private Collider2D feetCol;
		[SerializeField]
		private Collider2D headCol;
		private float jumpStartTime = 0.0f;
		private Vector3 respawnPosition = Vector3.zero;
		#endregion
		#region Public properties
		public bool isJumping { get; private set; } = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb2d = GetComponent<Rigidbody2D>();
		}
		void Start()
		{
			RefreshRespawnPosition();
		}
		void Update()
		{
			if(
				!isJumping &&
				Input.GetButtonDown("Jump")
			)
			{
				isJumping = true;
				jumpStartTime = Time.time;
			}
			if(transform.position.y <= killY)
				Respawn();
		}
		void FixedUpdate()
		{
			Vector2 force = new Vector2();
			//	Handle move
			force.x = Input.GetAxis("Horizontal") * moveSpeed;
			if(ignoreMass)
				force.x *= rb2d.mass;
			//	Handle jump
			if(
				isJumping &&
				Input.GetButton("Jump") &&
				Time.time - jumpStartTime <= maxJumpBoost
			)
				force.y = jumpSpeed;
			else
				force.y = 0.0f;
			//	Apply force
			rb2d.AddForce(force * Time.fixedDeltaTime, ForceMode2D.Impulse);
			//	Clamp velocity
			Vector2 vel = rb2d.velocity;
			vel.x = Mathf.Sign(vel.x) * Mathf.Min(maxWalkSpeed, Mathf.Abs(vel.x));
			vel.y = Mathf.Max(-maxFallSpeed, vel.y);
			rb2d.velocity = vel;
		}
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(
				collision.otherCollider == feetCol &&
				Vector2.Dot(Vector2.up, collision.GetContact(0).normal) > 0.85f
			)
			{   //	Feet contact below
				isJumping = false;
			}
			else if(
				collision.otherCollider == headCol &&
				collision.enabled &&
				Vector2.Dot(Vector2.down, collision.GetContact(0).normal) > 0.85f
			)
			{   //	Headbutt rom above
				jumpStartTime = 0.0f;
				Vector3 vel = rb2d.velocity;
				vel.y = 0.0f;
				rb2d.velocity = vel;
			}
		}
		void OnTriggerEnter2D(Collider2D collider)
		{
			if(collider.CompareTag("Check Point"))
				RefreshRespawnPosition(collider.transform.position);
		}
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		private void Respawn()
		{
			transform.position = respawnPosition;
			rb2d.velocity = Vector2.zero;
		}
		private void RefreshRespawnPosition(Vector3? position = null) { respawnPosition = position != null ? position.Value : transform.position; }
		#endregion
	}
}
