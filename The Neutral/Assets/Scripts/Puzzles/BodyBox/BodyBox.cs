using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class BodyBox : MonoBehaviour {

		public Lite boxColor;

		private Vector3 offset;
		private Vector3 targetPos;

		private Transform collisionPoints;
		private Transform center;
        [SerializeField]
		private Transform playerTransform; 

		private SpriteRenderer centerSprite;
		private float centerThreshold;
		private float smoothing = 2f;
		private bool isFollowingPlayer;

		void Start () {

			// Instantiate Center Attributes
			center = transform.GetChild (0);
			centerSprite = center.gameObject.GetComponent<SpriteRenderer> ();
			centerThreshold = 2f;

			// Instantiate Collision Points
			collisionPoints = transform.GetChild(0);
		}

		// TODO: remove after debugging
		private void handleInput()
		{
			if (Input.GetKeyDown(KeyCode.Comma))
			{
				setFollowingPlayer ();
			}
		}

		/// <summary>
		/// If the player is close enough to box center and the correct input
		/// is given, the box will begin following the player
		/// </summary>
		private void setFollowingPlayer()
		{
			if (playerTransform == null) {
				offset = Vector3.zero;
				isFollowingPlayer = false;
				return;
			}

			//if (checkPlayerCentered ()) {
				isFollowingPlayer = !isFollowingPlayer;
				offset = transform.position - playerTransform.position;
			//}
		}
			
		/// <summary>
		/// Called per frame. Has the box match the positioning
		/// of the player, to an offset.
		/// </summary>
		private void followPlayer()
		{
			if (playerTransform == null) 
			{
				return;
			}

			targetPos = playerTransform.position + offset;
			transform.position = Vector3.Lerp (transform.position, targetPos, smoothing * Time.deltaTime);

		}

		/// <summary>
		/// Returns true if player is close enough to the box's center
		/// </summary>
		/// <returns><c>true</c>, if player centered was checked, <c>false</c> otherwise.</returns>
		private bool checkPlayerCentered()
		{
			var xCheck = Mathf.Abs(playerTransform.position.x - center.position.x) <= centerThreshold;
			var zCheck = Mathf.Abs(playerTransform.position.z - center.position.z) <= centerThreshold;

			return xCheck && zCheck;
		}

		#region Collision Handling
		/// <summary>
		/// Loops through each collision point, checking if any have entered 
		/// another collider bounds
		/// </summary>
		private void checkCollisions ()
		{
			foreach (Transform collPoint in collisionPoints) 
			{
				if (collPoint.GetComponent<CollisionPoint>().hasCollided) 
				{
					ricochet (collPoint.forward);
					isFollowingPlayer = false;
					break;
				}
			}
		}

		/// <summary>
		/// Pushes the body box back upon collision
		/// </summary>
		/// <param name="pMotionVector">P motion vector.</param>
		private void ricochet(Vector3 pMotionVector)
		{
			if (!isFollowingPlayer)
				return;
			
			var newPos = transform.position;
			newPos -= pMotionVector;

			transform.position = newPos;
		}
			
		public void OnTriggerStay(Collider col)
		{
			if (col.CompareTag ("Player")) {
				playerTransform = col.transform;
			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.CompareTag ("Player")) 
			{
				playerTransform = null;
			}
		}
		#endregion

		void Update () {

			handleInput ();
			checkCollisions ();

			if (isFollowingPlayer) 
			{
				followPlayer ();
			}
		}
	}
}