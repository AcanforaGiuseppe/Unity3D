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
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Animator))]
	public class Movement : MonoBehaviour
	{
		#region Public variables
		public float moveAcceleration = 30.0f;
		public float maxMoveSpeed = 5.0f;
		#endregion
		#region Private variables
		private Rigidbody2D rb = null;
		private Collider2D col = null;
		private Animator animator = null;
		private SpriteRenderer spriteRenderer = null;
		private int keys = 0;
		private Vector3 respawnPosition = Vector3.zero;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			col = GetComponent<Collider2D>();
			animator = GetComponent<Animator>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		void Start()
		{
			StoreRespawnPosition();
		}
		void FixedUpdate()
		{
			rb.AddForce(
				new Vector2(
					Input.GetAxis("Horizontal"),
					Input.GetAxis("Vertical")
				) *
				Time.fixedDeltaTime *
				moveAcceleration,
				ForceMode2D.Impulse
			);
			rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxMoveSpeed);
			animator.SetBool("Moving", rb.velocity.sqrMagnitude > 0.01f);
			bool desiredFlip = rb.velocity.x < 0.0f;
			if(
				Mathf.Abs(rb.velocity.x) > 0.0f &&
				desiredFlip != spriteRenderer.flipX
			)
				spriteRenderer.flipX = desiredFlip;
		}
		void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.collider.CompareTag("Key"))
			{
				keys++;
				Destroy(collision.gameObject);
			}
			else if(
				collision.collider.CompareTag("Lock") &&
				keys > 0
			)
			{
				keys--;
				Destroy(collision.gameObject);
			}
			else if(collision.collider.CompareTag("Death"))
				Die();
		}
		void OnTriggerEnter2D(Collider2D collider)
		{
			if(collider.CompareTag("Check Point"))
				StoreRespawnPosition(collider.transform.position);
		}
		#endregion
		#region Public methods
		public void StoreRespawnPosition(Vector3? location = null)
		{
			respawnPosition = location != null ? location.Value : transform.position;
		}
		#endregion
		#region Private methods
		private void Die()
		{
			transform.position = respawnPosition;
		}
		#endregion
	}
}
