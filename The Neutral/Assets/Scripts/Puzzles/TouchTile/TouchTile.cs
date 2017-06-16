using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchTile : MonoBehaviour {

	public bool isMarkedTile = false;

	public void OnTriggerEnter(Collider pColl)
	{
		if (pColl.CompareTag("Player") && isMarkedTile)
		{
			transform.parent.GetComponent<TouchTileField> ().touched (transform);
		}
	}
}
