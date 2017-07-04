using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBox : MonoBehaviour {

	private Vector3 offset;
	private Vector3 targetPos;
	private Vector3 collisionPos;

	private Transform center;
	private SpriteRenderer centerSprite;
	private float centerThreshold;

	private Transform playerTransform; 

	private bool isColliding;
	private bool isFollowingPlayer;
	private float smoothing = 9f;

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

	private void handleCollision(string pObjectName)
	{
		Vector3 newPos = transform.position;
		float bounceVal = 1.5f;
		switch (pObjectName) 
		{
		case "Wall_North":
			newPos.z -= bounceVal;
			break;
		case "Wall_East":
			newPos.x -= bounceVal;
			break;
		case "Wall_South":
			newPos.z += bounceVal;
			break;
		case "Wall_West":
			newPos.x += bounceVal;
			break;
		default:
			break;
		}
		collisionPos = newPos;
		isFollowingPlayer = false;
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) {
			playerTransform = col.transform;
		} else {
			handleCollision (col.gameObject.name);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		if (col.CompareTag ("Player")) {
			if (!checkPlayerCentered ()) {
				isFollowingPlayer = false;
			}
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

		if (isFollowingPlayer) 
		{
			followPlayer ();
		}
	}
}
