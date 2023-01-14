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
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class Movement : MonoBehaviour
	{
		#region Public variables
		public float walkSpeed = 1.0f;
		public float jumpSpeed = 3.0f;
		public float maxJumpBoost = 0.35f;
		public float clampWalkVelocity = 4.0f;
		public float clampFallVelocity = 8.0f;
		public float killAltitude = -2.5f;
		#endregion
		#region Private variables
		private Rigidbody2D rb = null;
		private BoxCollider2D head = null;
		private CircleCollider2D feet = null;
		private Vector2 force = Vector2.zero;
		private float jumpStartTime = 0.0f;
		private Vector3 respawnPosition = Vector3.zero;
		private int keys = 0;
		#endregion
		#region Public properties
		public bool isJumping { get; private set; } = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			head = GetComponent<BoxCollider2D>();
			feet = GetComponent<CircleCollider2D>();
		}
		void Start()
		{
			respawnPosition = transform.position;
		}
		void Update()
		{
			force.x = Input.GetAxis("Horizontal") * walkSpeed;
			if(
				Input.GetButtonDown("Jump") &&
				!isJumping
			)
			{
				isJumping = true;
				jumpStartTime = Time.time;
			}
			if(
				isJumping &&
				Input.GetButton("Jump") &&
				Time.time - jumpStartTime <= maxJumpBoost
			)
				force.y = jumpSpeed;
			else
				force.y = 0.0f;
		}
		void FixedUpdate()
		{
			rb.AddForce(force * rb.mass * Time.fixedDeltaTime, ForceMode2D.Impulse);
			Vector3 clampedVelocity = rb.velocity;
			if(
				clampWalkVelocity > Mathf.Epsilon &&
				Mathf.Abs(clampedVelocity.x) > clampWalkVelocity
			)
				clampedVelocity.x = Mathf.Sign(clampedVelocity.x) * clampWalkVelocity;
			if(
				clampFallVelocity > Mathf.Epsilon &&
				clampedVelocity.y < -clampFallVelocity
			)
				clampedVelocity.y = -clampFallVelocity;
			rb.velocity = clampedVelocity;
		}
		void LateUpdate()
		{
			if(transform.position.y <= killAltitude)
				Respawn();
		}
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(
				collision.otherCollider == feet &&
				Vector2.Dot(Vector2.up, collision.GetContact(0).normal) > 0.85f
			)
			{   //	Feet contact below
				isJumping = false;
			}
			else if(
				collision.otherCollider == head &&
				collision.enabled &&
				Vector2.Dot(Vector2.down, collision.GetContact(0).normal) > 0.85f
			)
			{   //	Headbutt rom above
				jumpStartTime = 0.0f;
				Vector3 vel = rb.velocity;
				vel.y = 0.0f;
				rb.velocity = vel;
			}

			if(collision.collider.CompareTag("Key"))
			{
				keys++;
				Destroy(collision.gameObject);
			}
			else if(collision.collider.CompareTag("Lock"))
			{
				if(keys > 0)
				{
					keys--;
					Destroy(collision.gameObject);
				}
			}
		}
		void OnTriggerEnter2D(Collider2D collider)
		{
			if(collider.CompareTag("Check Point"))
				respawnPosition = collider.transform.position;
			else if(collider.CompareTag("Death"))
				Respawn();
		}
		#endregion
		#region Private methods
		private void Respawn()
		{
			rb.isKinematic = true;
			transform.position = respawnPosition;
			rb.velocity = Vector2.zero;
			rb.isKinematic = false;
		}
		#endregion
	}
}
