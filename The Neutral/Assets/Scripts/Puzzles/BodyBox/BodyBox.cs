using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBox : MonoBehaviour {

	public string boxColor;

	private Vector3 offset;
	private Vector3 targetPos;
	private Vector3 collisionPos;

	[SerializeField]
	private Transform collisionPoints;

	private Transform center;
	private SpriteRenderer centerSprite;
	private float centerThreshold;

	[SerializeField]
	private Transform playerTransform; 

	private bool isColliding;
	private bool isFollowingPlayer;
	private bool isHandlingCollision;
	private float smoothing = 2f;

	// Use this for initialization
	void Start () {
		center = transform.GetChild (0);
		centerSprite = center.gameObject.GetComponent<SpriteRenderer> ();
		centerThreshold = 2f;
	}


	private void handleInput()
	{
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			setFollowingPlayer ();
		}
	}

	private void setFollowingPlayer()
	{
		if (playerTransform == null) {
			offset = Vector3.zero;
			isFollowingPlayer = false;
			return;
		}

		if (checkPlayerCentered ()) {
			isFollowingPlayer = !isFollowingPlayer;
			offset = transform.position - playerTransform.position;
		}
	}

	private bool checkPlayerCentered()
	{
		bool xCheck = Mathf.Abs(playerTransform.position.x - center.position.x) <= centerThreshold;
		bool zCheck = Mathf.Abs(playerTransform.position.z - center.position.z) <= centerThreshold;

		return xCheck && zCheck;
	}

	private void followPlayer()
	{
		if (playerTransform == null) 
		{
			return;
		}

		targetPos = playerTransform.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetPos, smoothing * Time.deltaTime);

	}

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

	private void ricochet(Vector3 pMotionVector)
	{
		if (!isFollowingPlayer)
			return;
		
		Vector3 newPos = transform.position;
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

	// Update is called once per frame
	void Update () {

		handleInput ();
		checkCollisions ();

		if (isFollowingPlayer) 
		{
			followPlayer ();
		}
	}
}
